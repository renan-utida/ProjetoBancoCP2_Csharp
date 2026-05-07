using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoBancoCP2.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_AGENCIA",
                columns: table => new
                {
                    ID_AGENCIA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_AGENCIA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CEP = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    DS_ENDERECO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AGENCIA", x => x.ID_AGENCIA);
                });

            migrationBuilder.CreateTable(
                name: "TB_PRODUTO",
                columns: table => new
                {
                    ID_PRODUTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_PRODUTO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TIPO_PRODUTO = table.Column<string>(type: "NVARCHAR2(13)", maxLength: 13, nullable: false),
                    VL_SOLICITADO = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true),
                    TX_JUROS = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: true),
                    NR_PRAZO_MESES = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    VL_PARCELA = table.Column<decimal>(type: "DECIMAL(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PRODUTO", x => x.ID_PRODUTO);
                });

            migrationBuilder.CreateTable(
                name: "TB_CLIENTE",
                columns: table => new
                {
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_CLIENTE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_AGENCIA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_CLIENTE = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: true),
                    DT_NASCIMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CNPJ = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: true),
                    RAZAO_SOCIAL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CLIENTE", x => x.ID_CLIENTE);
                    table.ForeignKey(
                        name: "FK_TB_CLIENTE_TB_AGENCIA_ID_AGENCIA",
                        column: x => x.ID_AGENCIA,
                        principalTable: "TB_AGENCIA",
                        principalColumn: "ID_AGENCIA",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONTRATACAO",
                columns: table => new
                {
                    ID_CONTRATACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_PRODUTO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DT_SOLICITACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONTRATACAO", x => x.ID_CONTRATACAO);
                    table.ForeignKey(
                        name: "FK_TB_CONTRATACAO_TB_CLIENTE_ID_CLIENTE",
                        column: x => x.ID_CLIENTE,
                        principalTable: "TB_CLIENTE",
                        principalColumn: "ID_CLIENTE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_CONTRATACAO_TB_PRODUTO_ID_PRODUTO",
                        column: x => x.ID_PRODUTO,
                        principalTable: "TB_PRODUTO",
                        principalColumn: "ID_PRODUTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_CLIENTE_ID_AGENCIA",
                table: "TB_CLIENTE",
                column: "ID_AGENCIA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTRATACAO_ID_CLIENTE",
                table: "TB_CONTRATACAO",
                column: "ID_CLIENTE");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTRATACAO_ID_PRODUTO",
                table: "TB_CONTRATACAO",
                column: "ID_PRODUTO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CONTRATACAO");

            migrationBuilder.DropTable(
                name: "TB_CLIENTE");

            migrationBuilder.DropTable(
                name: "TB_PRODUTO");

            migrationBuilder.DropTable(
                name: "TB_AGENCIA");
        }
    }
}
