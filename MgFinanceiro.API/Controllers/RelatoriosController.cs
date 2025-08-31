using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MgFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;
    
    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService ?? throw new ArgumentNullException(nameof(relatorioService));
    }

    /// <summary>
    /// Obtém um resumo de transações por período.
    /// </summary>
    /// <param name="ano">Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.</param>
    /// <param name="mes">Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.</param>
    /// <returns>Retorna um resumo com saldo total, receitas e despesas por mês.</returns>
    /// <response code="200">Retorna a lista de resumos de transações.</response>
    /// <response code="400">Retorna erros de validação ou falha na geração do relatório.</response>
    /// <response code="401">Acesso não autorizado.</response>
    [HttpGet("resumo")]
    [SwaggerOperation(
        Summary = "Obtém um resumo de transações por período",
        Description = "Retorna um resumo com saldo total, receitas e despesas para o ano atual (todos os meses) ou para um mês/ano específico.",
        Tags = new[] { "Relatórios" }
    )]
    [ProducesResponseType(typeof(IEnumerable<RelatorioResumoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetResumo(
        [FromQuery, SwaggerParameter("Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.", Required = false)]
        int? ano,
        [FromQuery, SwaggerParameter("Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.", Required = false)]
        int? mes)
    {
        var query = new RelatorioQueryDto { Ano = ano, Mes = mes };
        var result = await _relatorioService.GetRelatorioResumoAsync(query);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Obtém transações agrupadas por categoria.
    /// </summary>
    /// <param name="ano">Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.</param>
    /// <param name="mes">Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.</param>
    /// <returns>Retorna transações agrupadas por categoria.</returns>
    /// <response code="200">Retorna a lista de transações agrupadas por categoria.</response>
    /// <response code="400">Retorna erros de validação ou falha na geração do relatório.</response>
    /// <response code="401">Acesso não autorizado.</response>
    [HttpGet("por-categoria")]
    [SwaggerOperation(
        Summary = "Obtém transações agrupadas por categoria",
        Description = "Retorna transações agrupadas por categoria para o ano atual (todos os meses) ou para um mês/ano específico.",
        Tags = new[] { "Relatórios" }
    )]
    [ProducesResponseType(typeof(IEnumerable<RelatorioPorCategoriaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPorCategoria(
        [FromQuery, SwaggerParameter("Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.", Required = false)]
        int? ano,
        [FromQuery, SwaggerParameter("Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.", Required = false)]
        int? mes)
    {
        var query = new RelatorioQueryDto { Ano = ano, Mes = mes };
        var result = await _relatorioService.GetRelatorioPorCategoriaAsync(query);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Exporta um resumo de transações em formato PDF ou Excel.
    /// </summary>
    /// <param name="ano">Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.</param>
    /// <param name="mes">Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.</param>
    /// <param name="formato">Formato do arquivo a ser exportado ('pdf' ou 'xlsx').</param>
    /// <returns>Retorna o arquivo de relatório no formato especificado.</returns>
    /// <response code="200">Retorna o arquivo PDF ou Excel com o resumo das transações.</response>
    /// <response code="400">Retorna erros de validação, formato inválido ou falha na geração do arquivo.</response>
    /// <response code="401">Acesso não autorizado.</response>
    [HttpPost("resumo/export")]
    [SwaggerOperation(
        Summary = "Exporta um resumo de transações",
        Description = "Gera e retorna um arquivo com o resumo de transações no formato PDF ou Excel para o período especificado.",
        Tags = new[] { "Relatórios" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ExportResumo(
        [FromQuery, SwaggerParameter("Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.", Required = false)]
        int? ano,
        [FromQuery, SwaggerParameter("Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.", Required = false)]
        int? mes,
        [FromQuery, SwaggerParameter("Formato do arquivo a ser exportado ('pdf' ou 'xlsx').", Required = true)]
        string formato)
    {
        var query = new RelatorioQueryDto { Ano = ano, Mes = mes };
        var result = await _relatorioService.ExportRelatorioResumoAsync(query, formato);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        var fileName = $"Relatorio_Resumo_{DateTime.Now:yyyyMMddHHmmss}.{formato.ToLower()}";
        var contentType = formato.ToLower() == "pdf"
            ? "application/pdf"
            : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(result.Value!, contentType, fileName);
    }

    /// <summary>
    /// Exporta transações agrupadas por categoria em formato PDF ou Excel.
    /// </summary>
    /// <param name="ano">Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.</param>
    /// <param name="mes">Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.</param>
    /// <param name="formato">Formato do arquivo a ser exportado ('pdf' ou 'xlsx').</param>
    /// <returns>Retorna o arquivo de relatório no formato especificado.</returns>
    /// <response code="200">Retorna o arquivo PDF ou Excel com as transações por categoria.</response>
    /// <response code="400">Retorna erros de validação, formato inválido ou falha na geração do arquivo.</response>
    /// <response code="401">Acesso não autorizado.</response>
    [HttpPost("por-categoria/export")]
    [SwaggerOperation(
        Summary = "Exporta transações por categoria",
        Description = "Gera e retorna um arquivo com transações agrupadas por categoria no formato PDF ou Excel para o período especificado.",
        Tags = new[] { "Relatórios" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ExportPorCategoria(
        [FromQuery, SwaggerParameter("Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.", Required = false)]
        int? ano,
        [FromQuery, SwaggerParameter("Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.", Required = false)]
        int? mes,
        [FromQuery, SwaggerParameter("Formato do arquivo a ser exportado ('pdf' ou 'xlsx').", Required = true)]
        string formato)
    {
        var query = new RelatorioQueryDto { Ano = ano, Mes = mes };
        var result = await _relatorioService.ExportRelatorioPorCategoriaAsync(query, formato);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        var fileName = $"Relatorio_PorCategoria_{DateTime.Now:yyyyMMddHHmmss}.{formato.ToLower()}";
        var contentType = formato.ToLower() == "pdf"
            ? "application/pdf"
            : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(result.Value!, contentType, fileName);
    }
}