using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MgFinanceiro.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Categorias",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7303));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7454));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7455));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7457));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7458));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 52, 59, 897, DateTimeKind.Utc).AddTicks(7459));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 21, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(1761), new DateTime(2025, 8, 26, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(1189) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 23, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(2125), new DateTime(2025, 8, 26, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(2123) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 16, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(2129), new DateTime(2025, 8, 26, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(2128) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 24, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(2131), new DateTime(2025, 8, 26, 23, 52, 59, 898, DateTimeKind.Utc).AddTicks(2130) });

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_Data",
                table: "Transacoes",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nome_Tipo",
                table: "Categorias",
                columns: new[] { "Nome", "Tipo" },
                unique: true,
                filter: "Ativo = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transacoes_Data",
                table: "Transacoes");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_Nome_Tipo",
                table: "Categorias");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8280));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8418));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8419));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8421));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8422));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8423));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8425));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8426));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8427));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 26, 23, 18, 59, 802, DateTimeKind.Utc).AddTicks(8428));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 21, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3236), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(2583) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 23, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3569), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3568) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 16, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3574), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3573) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 24, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3575), new DateTime(2025, 8, 26, 23, 18, 59, 803, DateTimeKind.Utc).AddTicks(3574) });
        }
    }
}
