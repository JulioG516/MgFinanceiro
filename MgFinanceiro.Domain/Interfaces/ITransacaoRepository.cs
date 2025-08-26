using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.Interfaces;

public interface ITransacaoRepository
{
    Task<IEnumerable<Transacao>> GetAllTransacoesAsync(DateTime? dataInicio, DateTime? dataFim, int? categoriaId,
        TipoCategoria? tipoCategoria);
    Task<Transacao?> GetTransacaoByIdAsync(int id);
    Task<Result> CreateTransacaoAsync(Transacao transacao);
    Task<Result> UpdateTransacaoAsync(Transacao transacao);
    Task<Result> DeleteTransacaoAsync(int id);
}