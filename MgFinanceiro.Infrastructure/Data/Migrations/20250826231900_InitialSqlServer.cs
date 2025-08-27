using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MgFinanceiro.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacoes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativo", "DataCriacao", "Nome", "Tipo" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8280), "Vendas de Produtos", 1 },
                    { 2, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8418), "Prestação de Serviços", 1 },
                    { 3, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8419), "Juros e Rendimentos Financeiros", 1 },
                    { 4, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8421), "Outras Receitas Operacionais", 1 },
                    { 5, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8422), "Salários e Encargos", 2 },
                    { 6, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8423), "Aluguel e Manutenção Predial", 2 },
                    { 7, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8425), "Compras de Mercadorias", 2 },
                    { 8, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8426), "Impostos e Taxas", 2 },
                    { 9, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8427), "Despesas com Marketing", 2 },
                    { 10, true, new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8428), "Despesas Administrativas", 2 }
                });

            migrationBuilder.InsertData(
                table: "Transacoes",
                columns: new[] { "Id", "CategoriaId", "Data", "DataCriacao", "Descricao", "Observacoes", "Valor" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 8, 21, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3236), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(2583), "Venda de produtos para cliente A", null, 1500.00m },
                    { 2, 2, new DateTime(2025, 8, 23, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3569), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3568), "Prestação de serviço de consultoria", null, 800.00m },
                    { 3, 5, new DateTime(2025, 8, 16, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3574), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3573), "Pagamento de salários mensais", null, 2000.00m },
                    { 4, 7, new DateTime(2025, 8, 24, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3575), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3574), "Compra de estoque", null, 1200.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_CategoriaId",
                table: "Transacoes",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
