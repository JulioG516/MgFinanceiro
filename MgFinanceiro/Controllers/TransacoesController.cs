using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MgFinanceiro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;

    public TransacoesController(ITransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim,
        [FromQuery] int? categoriaId, [FromQuery] TipoCategoria? tipoCategoria)
    {
        var transacoes = await _transacaoService.GetAllTransacoesAsync(dataInicio, dataFim, categoriaId, tipoCategoria);
        return Ok(transacoes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transacao = await _transacaoService.GetTransacaoByIdAsync(id);
        if (transacao == null)
        {
            return NotFound();
        }

        return Ok(transacao);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransacaoRequest request)
    {
        var result = await _transacaoService.CreateTransacaoAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = request.CategoriaId }, request);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTransacaoRequest request)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _transacaoService.DeleteTransacaoAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }
}