using MgFinanceiro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MgFinanceiro.Infrastructure.Data;

public class SeedData
{
    public static void Initialize(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().HasData(
            // Receitas
            new Categoria
            {
                Id = 1,
                Nome = "Vendas de Produtos",
                Tipo = TipoCategoria.Receita,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 2,
                Nome = "Prestação de Serviços",
                Tipo = TipoCategoria.Receita,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 3,
                Nome = "Juros e Rendimentos Financeiros",
                Tipo = TipoCategoria.Receita,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 4,
                Nome = "Outras Receitas Operacionais",
                Tipo = TipoCategoria.Receita,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            // Despesas
            new Categoria
            {
                Id = 5,
                Nome = "Salários e Encargos",
                Tipo = TipoCategoria.Despesa,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 6,
                Nome = "Aluguel e Manutenção Predial",
                Tipo = TipoCategoria.Despesa,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 7,
                Nome = "Compras de Mercadorias",
                Tipo = TipoCategoria.Despesa,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 8,
                Nome = "Impostos e Taxas",
                Tipo = TipoCategoria.Despesa,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 9,
                Nome = "Despesas com Marketing",
                Tipo = TipoCategoria.Despesa,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Categoria
            {
                Id = 10,
                Nome = "Despesas Administrativas",
                Tipo = TipoCategoria.Despesa,
                Ativo = true,
                DataCriacao = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            });

        // Seed de Transacoes
        modelBuilder.Entity<Transacao>().HasData(
            new Transacao
            {
                Id = 1,
                Descricao = "Venda de produtos para cliente A",
                Valor = 1500.00m,
                Data = new DateTime(2025, 8, 23, 0, 0, 0, DateTimeKind.Utc),
                CategoriaId = 1
            },
            new Transacao
            {
                Id = 2,
                Descricao = "Prestação de serviço de consultoria",
                Valor = 800.00m,
                Data = new DateTime(2025, 8, 25, 0, 0, 0, DateTimeKind.Utc),
                CategoriaId = 2
            },
            new Transacao
            {
                Id = 3,
                Descricao = "Pagamento de salários mensais",
                Valor = 2000.00m,
                Data = new DateTime(2025, 8, 18, 0, 0, 0, DateTimeKind.Utc),
                CategoriaId = 5
            },
            new Transacao
            {
                Id = 4,
                Descricao = "Compra de estoque",
                Valor = 1200.00m,
                Data = new DateTime(2025, 8, 26, 0, 0, 0, DateTimeKind.Utc),
                CategoriaId = 7
            }
        );
    }
}