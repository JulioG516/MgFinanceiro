using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MgFinanceiro.Infrastructure.Repositories;

public class RelatorioRepository : IRelatorioRepository
{
    private readonly AppDbContext _context;

    public RelatorioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RelatorioResumo>> GetResumoAnoAtualAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var query = _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Categoria)
            .Where(t => t.Categoria != null && t.Categoria.Ativo && t.Data.Year == currentYear);

        var result = await query
            .GroupBy(t => new { t.Data.Year, t.Data.Month })
            .Select(g => new RelatorioResumo
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                SaldoTotal = g.Sum(t => t.Categoria.Tipo == TipoCategoria.Receita ? t.Valor : -t.Valor),
                TotalReceitas = g.Where(t => t.Categoria.Tipo == TipoCategoria.Receita).Sum(t => t.Valor),
                TotalDespesas = g.Where(t => t.Categoria.Tipo == TipoCategoria.Despesa).Sum(t => t.Valor)
            })
            .OrderBy(r => r.Ano)
            .ThenBy(r => r.Mes)
            .ToListAsync();

        // Para ter todos os meses
        var allMonths = Enumerable.Range(1, 12).Select(m => new RelatorioResumo
        {
            Ano = currentYear,
            Mes = m,
            SaldoTotal = 0,
            TotalReceitas = 0,
            TotalDespesas = 0
        }).ToList();

        foreach (var month in allMonths)
        {
            if (result.All(r => r.Mes != month.Mes))
            {
                result.Add(month);
            }
        }

        return result.OrderBy(r => r.Ano).ThenBy(r => r.Mes);
    }

    public async Task<IEnumerable<RelatorioResumo>> GetResumoPorPeriodoAsync(int ano, int? mes)
    {
        var query = _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Categoria)
            .Where(t => t.Categoria != null && t.Categoria.Ativo && t.Data.Year == ano);

        if (mes.HasValue)
        {
            query = query.Where(t => t.Data.Month == mes.Value);
        }

        var result = await query
            .GroupBy(t => new { t.Data.Year, t.Data.Month })
            .Select(g => new RelatorioResumo
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                SaldoTotal = g.Sum(t => t.Categoria.Tipo == TipoCategoria.Receita ? t.Valor : -t.Valor),
                TotalReceitas = g.Where(t => t.Categoria.Tipo == TipoCategoria.Receita).Sum(t => t.Valor),
                TotalDespesas = g.Where(t => t.Categoria.Tipo == TipoCategoria.Despesa).Sum(t => t.Valor)
            })
            .OrderBy(r => r.Ano)
            .ThenBy(r => r.Mes)
            .ToListAsync();

        if (!mes.HasValue)
        {
            var allMonths = Enumerable.Range(1, 12).Select(m => new RelatorioResumo
            {
                Ano = ano,
                Mes = m,
                SaldoTotal = 0,
                TotalReceitas = 0,
                TotalDespesas = 0
            }).ToList();

            foreach (var month in allMonths)
            {
                if (!result.Any(r => r.Mes == month.Mes))
                {
                    result.Add(month);
                }
            }

            result = result.OrderBy(r => r.Ano).ThenBy(r => r.Mes).ToList();
        }

        return result;
    }

    public async Task<IEnumerable<RelatorioPorCategoria>> GetPorCategoriaAnoAtualAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var query = _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Categoria)
            .Where(t => t.Categoria != null && t.Categoria.Ativo && t.Data.Year == currentYear);

        return await query
            .GroupBy(t => new { t.Data.Year, t.Data.Month, t.CategoriaId, t.Categoria.Nome, t.Categoria.Tipo })
            .Select(g => new RelatorioPorCategoria
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                CategoriaId = g.Key.CategoriaId,
                CategoriaNome = g.Key.Nome,
                CategoriaTipo = g.Key.Tipo,
                Total = g.Sum(t => t.Valor)
            })
            .OrderBy(r => r.Ano)
            .ThenBy(r => r.Mes)
            .ThenBy(r => r.CategoriaNome)
            .ToListAsync();
    }

    public async Task<IEnumerable<RelatorioPorCategoria>> GetPorCategoriaPorPeriodoAsync(int ano, int? mes)
    {
        var query = _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Categoria)
            .Where(t => t.Categoria != null && t.Categoria.Ativo && t.Data.Year == ano);

        if (mes.HasValue)
        {
            query = query.Where(t => t.Data.Month == mes.Value);
        }

        return await query
            .GroupBy(t => new { t.Data.Year, t.Data.Month, t.CategoriaId, t.Categoria.Nome, t.Categoria.Tipo })
            .Select(g => new RelatorioPorCategoria
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                CategoriaId = g.Key.CategoriaId,
                CategoriaNome = g.Key.Nome,
                CategoriaTipo = g.Key.Tipo,
                Total = g.Sum(t => t.Valor)
            })
            .OrderBy(r => r.Ano)
            .ThenBy(r => r.Mes)
            .ThenBy(r => r.CategoriaNome)
            .ToListAsync();
    }
}