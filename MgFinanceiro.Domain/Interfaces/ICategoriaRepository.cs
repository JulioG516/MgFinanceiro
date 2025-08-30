using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllCategorias(TipoCategoria? tipoCategoria = null,
        bool? statusCategoriaAtivo = null);

    Task<Categoria?> GetCategoriaByIdAsync(int id);
    Task<Result<Categoria>> CreateCategoria(Categoria categoria);
    Task<Result<Categoria>> UpdateCategoria(Categoria categoria);
}