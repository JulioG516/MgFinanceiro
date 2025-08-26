using MgFinanceiro.Application.DTOs;
using MgFinanceiro.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MgFinanceiro.Application.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class CategoriaMapper
{
    public static partial Categoria CreateCategoriaRequestToCategoria(CreateCategoriaRequest request);

    [MapperIgnoreSource(nameof(Categoria.Transacoes))]
    public static partial CategoriaResponseDto CategoriaToCategoriaResponse(Categoria categoria);

    public static partial IEnumerable<CategoriaResponseDto> CategoriasToCategoriaResponses(IEnumerable<Categoria> categorias);
}