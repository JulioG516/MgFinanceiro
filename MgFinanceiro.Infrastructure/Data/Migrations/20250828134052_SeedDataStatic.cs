using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MgFinanceiro.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataStatic : Migration
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

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 7,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 8,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 9,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 10,
                column: "DataCriacao",
                value: new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6124) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6581) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6583) });

            migrationBuilder.UpdateData(
                table: "Transacoes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Data", "DataCriacao" },
                values: new object[] { new DateTime(2025, 8, 26, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 28, 13, 40, 52, 418, DateTimeKind.Utc).AddTicks(6584) });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");

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
