using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllCategorias(TipoCategoria? tipoCategoria = null);
    Task<Categoria?> GetCategoriaByIdAsync(int id);
    Task<Result> CreateCategoria(Categoria categoria);
}