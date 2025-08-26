using MgFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MgFinanceiro.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        SeedData.Initialize(modelBuilder);
    }
}