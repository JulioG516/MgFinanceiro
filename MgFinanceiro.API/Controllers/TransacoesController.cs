using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MgFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    /// <summary>
    /// Retorna todas as transações cadastradas com suporte a paginação.
    /// </summary>
    /// <param name="dataInicio">Data inicial para filtro (formato ISO 8601, ex.: 2025-08-01T00:00:00Z).</param>
    /// <param name="dataFim">Data final para filtro (formato ISO 8601, ex.: 2025-08-31T23:59:59Z).</param>
    /// <param name="categoriaId">ID da categoria para filtro.</param>
    /// <param name="tipoCategoria">Tipo da categoria: 1 - Receita ou 2 - Despesa.</param>
    /// <param name="pageNumber">Número da página a ser retornada (mínimo 1, padrão: 1).</param>
    /// <param name="pageSize">Quantidade de itens por página (mínimo 1, padrão: 10, máximo: 100).</param>
    /// <returns>Lista paginada de transações com metadados de paginação.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar transações",
        Description = "Retorna todas as transações cadastradas com suporte a paginação. " +
                      "Pode ser filtrado por período (dataInicio e dataFim), categoria (categoriaId) e tipo de categoria (1 - Receita ou 2 - Despesa). " +
                      "Os parâmetros de paginação permitem controlar o número da página e o tamanho da página."
    )]
    [ProducesResponseType(typeof(PagedResponse<TransacaoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery,
         SwaggerParameter("Data inicial para filtro das transações (formato ISO 8601, ex.: 2025-08-01T00:00:00Z).",
             Required = false)]
        DateTime? dataInicio,
        [FromQuery,
         SwaggerParameter("Data final para filtro das transações (formato ISO 8601, ex.: 2025-08-31T23:59:59Z).",
             Required = false)]
        DateTime? dataFim,
        [FromQuery, SwaggerParameter("ID da categoria para filtro das transações.", Required = false)]
        int? categoriaId,
        [FromQuery, SwaggerParameter("Tipo da categoria para filtro: 1 (Receita) ou 2 (Despesa).", Required = false)]
        TipoCategoria? tipoCategoria,
        [FromQuery, SwaggerParameter("Número da página a ser retornada (mínimo 1, padrão: 1).", Required = false)]
        int pageNumber = 1,
        [FromQuery,
         SwaggerParameter("Quantidade de itens por página (mínimo 1, padrão: 10, máximo: 100).", Required = false)]
        int pageSize = 10)
    {
        var transacoes = await _transacaoService.GetAllTransacoesAsync(dataInicio, dataFim, categoriaId, tipoCategoria,
            pageNumber, pageSize);
        return Ok(transacoes);
    }

    /// <summary>
    /// Retorna uma transação específica pelo ID.
    /// </summary>
    /// <param name="id">ID da transação.</param>
    /// <returns>Detalhes da transação.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obter transação por ID",
        Description = "Retorna os detalhes de uma transação específica com base no ID fornecido."
    )]
    [ProducesResponseType(typeof(TransacaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute, SwaggerParameter("ID da transação.", Required = true)]
        int id)
    {
        var transacao = await _transacaoService.GetTransacaoByIdAsync(id);
        if (transacao == null)
        {
            return NotFound("Transação não encontrada.");
        }

        return Ok(transacao);
    }

    /// <summary>
    /// Cria uma nova transação.
    /// </summary>
    /// <param name="request">Dados da transação a ser criada.</param>
    /// <returns>Transação criada.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Criar transação",
        Description = "Cria uma nova transação com descrição, valor, data, categoria e observações opcionais."
    )]
    [ProducesResponseType(typeof(TransacaoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody, SwaggerParameter("Dados da transação a ser criada.", Required = true)]
        CreateTransacaoRequest request)
    {
        var result = await _transacaoService.CreateTransacaoAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>
    /// Atualiza uma transação existente.
    /// </summary>
    /// <param name="id">ID da transação a ser atualizada.</param>
    /// <param name="request">Dados atualizados da transação.</param>
    /// <returns>Nenhum conteúdo se bem-sucedido.</returns>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Atualizar transação",
        Description = "Atualiza os detalhes de uma transação existente com base no ID fornecido."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromRoute, SwaggerParameter("ID da transação a ser atualizada.", Required = true)]
        int id,
        [FromBody, SwaggerParameter("Dados atualizados da transação.", Required = true)]
        UpdateTransacaoRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("O ID da transação na URL deve corresponder ao ID no corpo da requisição.");
        }

        var result = await _transacaoService.UpdateTransacaoAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Exclui uma transação pelo ID.
    /// </summary>
    /// <param name="id">ID da transação a ser excluída.</param>
    /// <returns>Nenhum conteúdo se bem-sucedido.</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Excluir transação",
        Description = "Exclui uma transação existente com base no ID fornecido."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromRoute, SwaggerParameter("ID da transação a ser excluída.", Required = true)]
        int id)
    {
        var result = await _transacaoService.DeleteTransacaoAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }
}