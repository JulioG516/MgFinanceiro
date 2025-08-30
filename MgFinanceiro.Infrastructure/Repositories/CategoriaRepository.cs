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

    public async Task<IEnumerable<Categoria>> GetAllCategorias(TipoCategoria? tipoCategoria = null,
        bool? statusCategoriaAtivo = null)
    {
        var query = _context.Categorias
            .AsNoTracking();

        if (tipoCategoria.HasValue)
        {
            query = query.Where(c => c.Tipo == tipoCategoria.Value);
        }

        if (statusCategoriaAtivo.HasValue)
        {
            query = query.Where(c => c.Ativo == statusCategoriaAtivo.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Categoria?> GetCategoriaByIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task<Result<Categoria>> CreateCategoria(Categoria categoria)
    {
        try
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return Result<Categoria>.Success(categoria);
        }
        catch (Exception ex)
        {
            return Result<Categoria>.Failure($"Erro ao criar categoria: {ex.Message}");
        }
    }

    public async Task<Result<Categoria>> UpdateCategoria(Categoria categoria)
    {
        try
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return Result<Categoria>.Success(categoria);
        }
        catch (Exception ex)
        {
            return Result<Categoria>.Failure($"Erro ao atualizar categoria: {ex.Message}");
        }
    }
}