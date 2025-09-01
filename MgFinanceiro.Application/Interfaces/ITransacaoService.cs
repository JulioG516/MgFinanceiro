using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.Interfaces;

public interface ITransacaoService
{
    Task<PagedResponse<TransacaoResponseDto>> GetAllTransacoesAsync(DateTime? dataInicio, DateTime? dataFim,
        int? categoriaId, TipoCategoria? tipoCategoria, int pageNumber = 1, int pageSize = 10);
    Task<TransacaoResponseDto?> GetTransacaoByIdAsync(int id);
    Task<Result<TransacaoResponseDto>> CreateTransacaoAsync(CreateTransacaoRequest request);
    Task<Result> UpdateTransacaoAsync(UpdateTransacaoRequest request);
    Task<Result> DeleteTransacaoAsync(int id);
}