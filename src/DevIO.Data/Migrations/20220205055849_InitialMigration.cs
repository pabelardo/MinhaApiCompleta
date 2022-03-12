using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevIO.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FORNECEDORES",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NOME = table.Column<string>(type: "varchar(200)", nullable: false),
                    DOCUMENTO = table.Column<string>(type: "varchar(14)", nullable: false),
                    TIPO_FORNECEDOR = table.Column<int>(type: "int", nullable: false),
                    ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORNECEDORES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ENDERECOS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FORNECEDOR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LOGRADOURO = table.Column<string>(type: "varchar(200)", nullable: false),
                    NUMERO = table.Column<string>(type: "varchar(50)", nullable: false),
                    COMPLEMENTO = table.Column<string>(type: "varchar(250)", nullable: true),
                    CEP = table.Column<string>(type: "varchar(8)", nullable: false),
                    BAIRRO = table.Column<string>(type: "varchar(100)", nullable: false),
                    CIDADE = table.Column<string>(type: "varchar(100)", nullable: false),
                    ESTADO = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENDERECOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ENDERECOS_FORNECEDORES_FORNECEDOR_ID",
                        column: x => x.FORNECEDOR_ID,
                        principalTable: "FORNECEDORES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTOS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FORNECEDOR_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NOME = table.Column<string>(type: "varchar(200)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "varchar(1000)", nullable: false),
                    IMAGEM = table.Column<string>(type: "varchar(100)", nullable: false),
                    VALOR = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DATA_CADASTRO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ATIVO = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PRODUTOS_FORNECEDORES_FORNECEDOR_ID",
                        column: x => x.FORNECEDOR_ID, 
                        principalTable: "FORNECEDORES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ENDERECOS_FORNECEDOR_ID",
                table: "ENDERECOS",
                column: "FORNECEDOR_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUTOS_FORNECEDOR_ID",
                table: "PRODUTOS",
                column: "FORNECEDOR_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ENDERECOS");

            migrationBuilder.DropTable(
                name: "PRODUTOS");

            migrationBuilder.DropTable(
                name: "FORNECEDORES");
        }
    }
}