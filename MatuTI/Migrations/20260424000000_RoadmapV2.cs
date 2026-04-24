using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatuTI.Migrations
{
    public partial class RoadmapV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NC2 — Mapeamento COBIT/ITIL nas questões
            migrationBuilder.AddColumn<string>(
                name: "DominioCOBIT",
                table: "Questoes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PraticaITIL",
                table: "Questoes",
                type: "TEXT",
                nullable: true);

            // NC3 — Plano de ação estruturado nas respostas
            migrationBuilder.AddColumn<string>(
                name: "PlanoResponsavel",
                table: "Respostas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanoPrazo",
                table: "Respostas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanoStatus",
                table: "Respostas",
                type: "TEXT",
                nullable: false,
                defaultValue: "Pendente");

            // NC4 — Upload de evidência
            migrationBuilder.AddColumn<string>(
                name: "EvidenciaArquivo",
                table: "Respostas",
                type: "TEXT",
                nullable: true);

            // NC5 — Inventário de ativos de TI
            migrationBuilder.CreateTable(
                name: "AtivosIT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmpresaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Fabricante = table.Column<string>(type: "TEXT", nullable: true),
                    Versao = table.Column<string>(type: "TEXT", nullable: true),
                    Localizacao = table.Column<string>(type: "TEXT", nullable: true),
                    Responsavel = table.Column<string>(type: "TEXT", nullable: true),
                    DataAquisicao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FimSuporte = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Critico = table.Column<bool>(type: "INTEGER", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtivosIT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtivosIT_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtivosIT_EmpresaId",
                table: "AtivosIT",
                column: "EmpresaId");

            // NC6 — Avaliação LGPD
            migrationBuilder.CreateTable(
                name: "AvaliacoesLGPD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AvaliacaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Questao = table.Column<string>(type: "TEXT", nullable: false),
                    Conforme = table.Column<bool>(type: "INTEGER", nullable: false),
                    Observacao = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacoesLGPD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacoesLGPD_Avaliacoes_AvaliacaoId",
                        column: x => x.AvaliacaoId,
                        principalTable: "Avaliacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesLGPD_AvaliacaoId",
                table: "AvaliacoesLGPD",
                column: "AvaliacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AtivosIT");
            migrationBuilder.DropTable(name: "AvaliacoesLGPD");
            migrationBuilder.DropColumn(name: "DominioCOBIT", table: "Questoes");
            migrationBuilder.DropColumn(name: "PraticaITIL", table: "Questoes");
            migrationBuilder.DropColumn(name: "PlanoResponsavel", table: "Respostas");
            migrationBuilder.DropColumn(name: "PlanoPrazo", table: "Respostas");
            migrationBuilder.DropColumn(name: "PlanoStatus", table: "Respostas");
            migrationBuilder.DropColumn(name: "EvidenciaArquivo", table: "Respostas");
        }
    }
}
