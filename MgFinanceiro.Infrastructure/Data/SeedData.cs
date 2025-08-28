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
                Id = 1, Descricao = "Venda de produtos para cliente A", Valor = 1500.00m,
                Data = new DateTime(2025, 8, 23, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 1,
                Observacoes = "Venda de 50 unidades de produto X, entrega realizada em 2 dias."
            },
            new Transacao
            {
                Id = 2, Descricao = "Prestação de serviço de consultoria", Valor = 800.00m,
                Data = new DateTime(2025, 8, 25, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 2,
                Observacoes = "Consultoria de 10 horas para otimização de processos."
            },
            new Transacao
            {
                Id = 3, Descricao = "Pagamento de salários mensais", Valor = 2000.00m,
                Data = new DateTime(2025, 8, 18, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 5,
                Observacoes = "Salários de 5 funcionários, pago via transferência bancária."
            },
            new Transacao
            {
                Id = 4, Descricao = "Compra de estoque", Valor = 1200.00m,
                Data = new DateTime(2025, 8, 26, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 7,
                Observacoes = "Aquisição de 100 unidades de matéria-prima Y."
            },
            new Transacao
            {
                Id = 5, Descricao = "Receita de aluguel de equipamento", Valor = 600.00m,
                Data = new DateTime(2025, 7, 10, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 4,
                Observacoes = "Aluguel de máquina por 5 dias para cliente externo."
            },
            new Transacao
            {
                Id = 6, Descricao = "Pagamento de aluguel mensal", Valor = 900.00m,
                Data = new DateTime(2025, 7, 5, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 6,
            },
            new Transacao
            {
                Id = 7, Descricao = "Venda de produtos para cliente B", Valor = 2300.00m,
                Data = new DateTime(2025, 9, 12, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 1,
                Observacoes = "Venda de 80 unidades de produto Z, pedido urgente."
            },
            new Transacao
            {
                Id = 8, Descricao = "Campanha de marketing online", Valor = 500.00m,
                Data = new DateTime(2025, 9, 15, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 9,
                Observacoes = "Anúncios no Google Ads para promoção de produto X."
            },
            new Transacao
            {
                Id = 9, Descricao = "Juros recebidos de aplicação", Valor = 300.00m,
                Data = new DateTime(2025, 8, 30, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 3,
                Observacoes = "Juros de aplicação financeira em fundo de renda fixa."
            },
            new Transacao
            {
                Id = 10, Descricao = "Pagamento de impostos municipais", Valor = 400.00m,
                Data = new DateTime(2025, 7, 20, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 8,
            },
            new Transacao
            {
                Id = 11, Descricao = "Serviço de manutenção de equipamentos", Valor = 700.00m,
                Data = new DateTime(2025, 6, 25, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 6,
                Observacoes = "Manutenção preventiva de 3 máquinas industriais."
            },
            new Transacao
            {
                Id = 12, Descricao = "Venda de produtos para cliente C", Valor = 1800.00m,
                Data = new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 1,
                Observacoes = "Venda de 60 unidades de produto Y, pagamento à vista."
            },
            new Transacao
            {
                Id = 13, Descricao = "Despesas com material de escritório", Valor = 200.00m,
                Data = new DateTime(2025, 8, 10, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 10,
                Observacoes = "Compra de papel, canetas e outros suprimentos."
            },
            new Transacao
            {
                Id = 14, Descricao = "Receita de consultoria estratégica", Valor = 1200.00m,
                Data = new DateTime(2025, 9, 5, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 2,
            },
            new Transacao
            {
                Id = 15, Descricao = "Compra de matérias-primas", Valor = 1500.00m,
                Data = new DateTime(2025, 7, 28, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 7,
                Observacoes = "Aquisição de matéria-prima para produção de produto Z."
            },
            new Transacao
            {
                Id = 16, Descricao = "Pagamento de taxas estaduais", Valor = 350.00m,
                Data = new DateTime(2025, 6, 30, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 8,
                Observacoes = "ICMS referente a vendas do mês de junho."
            },
            new Transacao
            {
                Id = 17, Descricao = "Receita de royalties", Valor = 1000.00m,
                Data = new DateTime(2025, 8, 5, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 4,
                Observacoes = "Royalties de licenciamento de software proprietário."
            },
            new Transacao
            {
                Id = 18, Descricao = "Manutenção de veículos da empresa", Valor = 600.00m,
                Data = new DateTime(2025, 9, 20, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 6,
            },
            new Transacao
            {
                Id = 19, Descricao = "Venda de produtos para cliente D", Valor = 2500.00m,
                Data = new DateTime(2025, 7, 15, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 1,
                Observacoes = "Venda de 100 unidades de produto X, entrega em 3 dias."
            },
            new Transacao
            {
                Id = 20, Descricao = "Despesas com treinamento de equipe", Valor = 800.00m,
                Data = new DateTime(2025, 8, 12, 0, 0, 0, DateTimeKind.Utc), CategoriaId = 10,
            });

        modelBuilder.Entity<Usuario>()
            .HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Jose Teste",
                    Email = "teste@exemplo.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("Teste@Senha"),
                    DataCriacao = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UltimoLogin = null,
                }
            );
    }
}