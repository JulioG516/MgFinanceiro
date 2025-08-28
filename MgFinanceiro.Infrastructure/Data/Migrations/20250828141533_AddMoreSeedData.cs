using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MgFinanceiro.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(1644));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2059));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2060));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2062));

            migrationBuilder.InsertData(
                table: "Transacoes",
                columns: new[] { "Id", "CategoriaId", "Data", "DataCriacao", "Descricao", "Observacoes", "Valor" },
                values: new object[,]
                {
                    { 5, 4, new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2063), "Receita de aluguel de equipamento", null, 600.00m },
                    { 6, 6, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2064), "Pagamento de aluguel mensal", null, 900.00m },
                    { 7, 1, new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2065), "Venda de produtos para cliente B", null, 2300.00m },
                    { 8, 9, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2066), "Campanha de marketing online", null, 500.00m },
                    { 9, 3, new DateTime(2025, 8, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2067), "Juros recebidos de aplicação", null, 300.00m },
                    { 10, 8, new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2078), "Pagamento de impostos municipais", null, 400.00m },
                    { 11, 6, new DateTime(2025, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2079), "Serviço de manutenção de equipamentos", null, 700.00m },
                    { 12, 1, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2080), "Venda de produtos para cliente C", null, 1800.00m },
                    { 13, 10, new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2081), "Despesas com material de escritório", null, 200.00m },
                    { 14, 2, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2082), "Receita de consultoria estratégica", null, 1200.00m },
                    { 15, 7, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2083), "Compra de matérias-primas", null, 1500.00m },
                    { 16, 8, new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2084), "Pagamento de taxas estaduais", null, 350.00m },
                    { 17, 4, new DateTime(2025, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2085), "Receita de royalties", null, 1000.00m },
                    { 18, 6, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2086), "Manutenção de veículos da empresa", null, 600.00m },
                    { 19, 1, new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2087), "Venda de produtos para cliente D", null, 2500.00m },
                    { 20, 10, new DateTime(2025, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2089), "Despesas com treinamento de equipe", null, 800.00m }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "DataCriacao", "Email", "Nome", "SenhaHash", "UltimoLogin" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "teste@exemplo.com", "Jose Teste", "$2a$11$XCcSVqyjoEBm8AQ/gsklV.zOihn8RisCV2OVT.c7StBOaNfpRMATi", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6124));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6581));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6583));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6584));
        }
    }
}
