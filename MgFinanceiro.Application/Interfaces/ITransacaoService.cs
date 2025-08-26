using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.Interfaces;

public interface ITransacaoService
{
    Task<IEnumerable<TransacaoResponseDto>> GetAllTransacoesAsync(DateTime? dataInicio, DateTime? dataFim,
        int? categoriaId, TipoCategoria? tipoCategoria);

    Task<TransacaoResponseDto?> GetTransacaoByIdAsync(int id);
    Task<Result> CreateTransacaoAsync(CreateTransacaoRequest request);
    Task<Result> UpdateTransacaoAsync(UpdateTransacaoRequest request);
    Task<Result> DeleteTransacaoAsync(int id);
}