using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Domain.Common;

namespace MgFinanceiro.Application.Interfaces;

public interface IRelatorioService
{
    Task<Result<IEnumerable<RelatorioResumoResponse>>> GetRelatorioResumoAsync(RelatorioQueryDto query);
    Task<Result<IEnumerable<RelatorioPorCategoriaResponse>>> GetRelatorioPorCategoriaAsync(RelatorioQueryDto query);
}