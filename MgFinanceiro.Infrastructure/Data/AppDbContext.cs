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

        // Configura o SQL Server para melhor otimizacao
        modelBuilder.Entity<Transacao>()
            .Property(x => x.Descricao)
            .HasMaxLength(200);
        modelBuilder.Entity<Transacao>()
            .Property(x => x.Observacoes)
            .HasMaxLength(500);
        modelBuilder.Entity<Transacao>()
            .Property(t => t.Valor)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Transacao>()
            .HasIndex(t => t.Data);

        modelBuilder.Entity<Categoria>()
            .Property(c => c.Nome)
            .HasMaxLength(100);

        // Index composto e unico quando esta ativo.
        modelBuilder.Entity<Categoria>()
            .HasIndex(c => new { c.Nome, c.Tipo })
            .IsUnique()
            .HasFilter("Ativo = 1");

        SeedData.Initialize(modelBuilder);
    }
}