using MgFinanceiro.Application.DTOs.Categoria;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.Interfaces;

public interface ICategoriaService
{
    Task<CategoriaResponseDto?> GetCategoriaByIdAsync(int id);

    Task<IEnumerable<CategoriaResponseDto>> GetAllCategoriasAsync(TipoCategoria? tipoCategoria = null,
        bool? statusCategoriaAtivo = null);

    Task<Result<CategoriaResponseDto>> CreateCategoriaAsync(CreateCategoriaRequest request);
    Task<Result<CategoriaResponseDto>> UpdateCategoriaStatusAsync(int id, UpdateCategoriaStatusRequest request);
}