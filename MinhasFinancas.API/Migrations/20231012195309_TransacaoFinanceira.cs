using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasFinancas.API.Migrations
{
    /// <inheritdoc />
    public partial class TransacaoFinanceira : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SENHA_HASH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TOKEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VALIDADE_TOKEN = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TRANSACAO_FINANCEIRA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DESCRICAO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DATA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VALOR = table.Column<double>(type: "float", nullable: false),
                    TIPO = table.Column<int>(type: "int", nullable: false),
                    ID_USUARIO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACAO_FINANCEIRA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TRANSACAO_FINANCEIRA_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACAO_FINANCEIRA_ID_USUARIO",
                table: "TRANSACAO_FINANCEIRA",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_EMAIL",
                table: "USUARIO",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_NOME",
                table: "USUARIO",
                column: "NOME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRANSACAO_FINANCEIRA");

            migrationBuilder.DropTable(
                name: "USUARIO");
        }
    }
}
