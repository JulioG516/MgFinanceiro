using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MgFinanceiro.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLengthConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Transacoes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Transacoes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Categorias",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4259));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4261));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4263));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4264));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4265));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4268));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4269));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4270));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(4271));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 22, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(8547), new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(7933) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 24, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(8983), new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(8981) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 17, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(8988), new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(8987) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 25, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(8990), new DateTime(2025, 8, 27, 0, 2, 0, 45, DateTimeKind.Utc).AddTicks(8989) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Transacoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Transacoes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Categorias",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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
        }
    }
}
