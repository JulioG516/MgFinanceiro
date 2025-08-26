using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MgFinanceiro.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativo", "DataCriacao", "Nome", "Tipo" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4169), "Vendas de Produtos", 1 },
                    { 2, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4238), "Prestação de Serviços", 1 },
                    { 3, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4239), "Juros e Rendimentos Financeiros", 1 },
                    { 4, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4241), "Outras Receitas Operacionais", 1 },
                    { 5, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4242), "Salários e Encargos", 2 },
                    { 6, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4243), "Aluguel e Manutenção Predial", 2 },
                    { 7, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4244), "Compras de Mercadorias", 2 },
                    { 8, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4245), "Impostos e Taxas", 2 },
                    { 9, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4275), "Despesas com Marketing", 2 },
                    { 10, true, new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(4277), "Despesas Administrativas", 2 }
                });

            migrationBuilder.InsertData(
                table: "Transacoes",
                columns: new[] { "Id", "CategoriaId", "Data", "DataCriacao", "Descricao", "Observacoes", "Valor" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 8, 21, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(7261), new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(6971), "Venda de produtos para cliente A", null, 1500.00m },
                    { 2, 2, new DateTime(2025, 8, 23, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(7463), new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(7462), "Prestação de serviço de consultoria", null, 800.00m },
                    { 3, 5, new DateTime(2025, 8, 16, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(7466), new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(7465), "Pagamento de salários mensais", null, 2000.00m },
                    { 4, 7, new DateTime(2025, 8, 24, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(7468), new DateTime(2025, 8, 26, 15, 32, 18, 513, DateTimeKind.Utc).AddTicks(7467), "Compra de estoque", null, 1200.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
