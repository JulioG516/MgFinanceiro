using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllCategorias(int? tipoCategoria = null);
    Task<Result> CreateCategoria(Categoria categoria);
}   