using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MgFinanceiro.Infrastructure.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transacao>> GetAllTransacoesAsync(DateTime? dataInicio, DateTime? dataFim,
        int? categoriaId, TipoCategoria? tipoCategoria)
    {
        var query = _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Categoria)
            .AsQueryable();

        if (dataInicio.HasValue)
        {
            query = query.Where(t => t.Data >= dataInicio.Value);
        }

        if (dataFim.HasValue)
        {
            query = query.Where(t => t.Data <= dataFim.Value);
        }

        if (categoriaId.HasValue)
        {
            query = query.Where(t => t.CategoriaId == categoriaId.Value);
        }

        if (tipoCategoria.HasValue)
        {
            query = query.Where(t => t.Categoria!.Tipo == tipoCategoria.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Transacao?> GetTransacaoByIdAsync(int id)
    {
        return await _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Categoria)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Result> CreateTransacaoAsync(Transacao transacao)
    {
        try
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao criar transação: {ex.Message}");
        }
    }

    public async Task<Result> UpdateTransacaoAsync(Transacao transacao)
    {
        try
        {
            _context.Transacoes.Update(transacao);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao atualizar transação: {ex.Message}");
        }
    }

    public async Task<Result> DeleteTransacaoAsync(int id)
    {
        try
        {
            var transacao = await _context.Transacoes.FindAsync(id);
            if (transacao == null)
            {
                return Result.Failure("Transação não encontrada.");
            }

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao excluir transação: {ex.Message}");
        }
    }
}