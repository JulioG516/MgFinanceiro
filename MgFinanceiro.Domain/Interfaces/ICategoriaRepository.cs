using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.Interfaces;

public interface ICategoriaRepository
{
    Task<(IEnumerable<Categoria>, int)> GetAllCategorias(int pageNumber, int pageSize,
        TipoCategoria? tipoCategoria = null,
        bool? statusCategoriaAtivo = null);
    public Task<bool> ExistsCategoriaAsync(string nome, TipoCategoria tipo);
    Task<Categoria?> GetCategoriaByIdAsync(int id);
    Task<Result<Categoria>> CreateCategoria(Categoria categoria);
    Task<Result<Categoria>> UpdateCategoria(Categoria categoria);
}