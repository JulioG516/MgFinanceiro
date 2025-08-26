using MgFinanceiro.Application.DTOs.Relatorio;
using MgFinanceiro.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MgFinanceiro.Application.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class RelatorioMapper
{
    public static partial IEnumerable<RelatorioResumoResponse> MapResumos(IEnumerable<RelatorioResumo> resumo);


    public static partial IEnumerable<RelatorioPorCategoriaResponse> MapPorCategorias(
        IEnumerable<RelatorioPorCategoria> relatorioPorCategorias);
}