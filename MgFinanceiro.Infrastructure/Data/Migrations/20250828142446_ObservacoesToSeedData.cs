using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MgFinanceiro.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ObservacoesToSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1287), "Venda de 50 unidades de produto X, entrega realizada em 2 dias." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1808), "Consultoria de 10 horas para otimização de processos." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1811), "Salários de 5 funcionários, pago via transferência bancária." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1812), "Aquisição de 100 unidades de matéria-prima Y." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1813), "Aluguel de máquina por 5 dias para cliente externo." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1814));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1815), "Venda de 80 unidades de produto Z, pedido urgente." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1817), "Anúncios no Google Ads para promoção de produto X." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1826), "Juros de aplicação financeira em fundo de renda fixa." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1828), "Manutenção preventiva de 3 máquinas industriais." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1830), "Venda de 60 unidades de produto Y, pagamento à vista." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1831), "Compra de papel, canetas e outros suprimentos." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 14,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1832));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1833), "Aquisição de matéria-prima para produção de produto Z." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1834), "ICMS referente a vendas do mês de junho." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1835), "Royalties de licenciamento de software proprietário." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 18,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1837));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1838), "Venda de 100 unidades de produto X, entrega em 3 dias." });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 20,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 24, 46, 444, DateTimeKind.Utc).AddTicks(1839));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2a$11$q4yBPFxw9Wo4G9BhzeVUVeN5BvUcrkYBcf7Lvjw6FRczlu.ie2S0W");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(1644), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2059), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2060), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2062), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2063), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2064));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2065), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2066), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2067), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2078));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2079), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2080), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2081), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 14,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2082));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2083), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2084), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2085), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 18,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2086));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DataCriacao", "Observacoes" },
                values: new object[] { new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2087), null });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 20,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 28, 14, 15, 33, 260, DateTimeKind.Utc).AddTicks(2089));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "SenhaHash",
                value: "$2a$11$XCcSVqyjoEBm8AQ/gsklV.zOihn8RisCV2OVT.c7StBOaNfpRMATi");
        }
    }
}
