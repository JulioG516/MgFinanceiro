using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MgFinanceiro.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllCategorias(TipoCategoria? tipoCategoria = null)
    {
        var query = _context.Categorias
            .AsNoTracking()
            .Where(c => c.Ativo);

        if (tipoCategoria.HasValue)
        {
            query = query.Where(c => c.Tipo == tipoCategoria.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Result> CreateCategoria(Categoria categoria)
    {
        try
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao criar categoria: {ex.Message}");
        }
    }
}