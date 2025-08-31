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
        _relatorioService = relatorioService;
    }


    /// <summary>
    /// Retorna um resumo de transações por período.
    /// </summary>
    /// <param name="ano">Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.</param>
    /// <param name="mes">Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.</param>
    /// <returns>Resumo com saldo total, receitas e despesas por mês.</returns>
    [HttpGet("resumo")]
    [SwaggerOperation(
        Summary = "Resumo de transações por período",
        Description =
            "Retorna um resumo com saldo total, receitas e despesas para o ano atual (todos os meses) ou para um mês/ano específico."
    )]
    [ProducesResponseType(typeof(IEnumerable<RelatorioResumoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetResumo(
        [FromQuery, SwaggerParameter("Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.", Required = false)]
        int? ano,
        [FromQuery,
         SwaggerParameter("Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.",
             Required = false)]
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
    /// Retorna transações agrupadas por categoria.
    /// </summary>
    /// <param name="ano">Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.</param>
    /// <param name="mes">Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.</param>
    /// <returns>Transações agrupadas por categoria.</returns>
    [HttpGet("por-categoria")]
    [SwaggerOperation(
        Summary = "Transações por categoria",
        Description =
            "Retorna transações agrupadas por categoria para o ano atual (todos os meses) ou para um mês/ano específico."
    )]
    [ProducesResponseType(typeof(IEnumerable<RelatorioPorCategoriaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPorCategoria(
        [FromQuery, SwaggerParameter("Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual.", Required = false)]
        int? ano,
        [FromQuery,
         SwaggerParameter("Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano.",
             Required = false)]
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
}