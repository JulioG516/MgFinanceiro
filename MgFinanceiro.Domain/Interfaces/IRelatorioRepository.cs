using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.Interfaces;

public interface IRelatorioRepository
{
    Task<IEnumerable<RelatorioResumo>> GetResumoAnoAtualAsync();
    Task<IEnumerable<RelatorioResumo>> GetResumoPorPeriodoAsync(int ano, int? mes);
    Task<IEnumerable<RelatorioPorCategoria>> GetPorCategoriaAnoAtualAsync();
    Task<IEnumerable<RelatorioPorCategoria>> GetPorCategoriaPorPeriodoAsync(int ano, int? mes);
}