using MgFinanceiro.Application.DTOs;
using MgFinanceiro.Domain.Common;

namespace MgFinanceiro.Application.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDto>> GetAllCategoriasAsync(int? tipoCategoria = null);
    Task<Result> CreateCategoriaAsync(CreateCategoriaRequest request);
}