using MgFinanceiro.Application.DTOs;
using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDto>> GetAllCategoriasAsync(TipoCategoria? tipoCategoria = null);
    Task<Result> CreateCategoriaAsync(CreateCategoriaRequest request);
}