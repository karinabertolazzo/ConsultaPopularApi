using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaPopularApi.Migrations
{
    public partial class CriarClinicaEspecialidadesERelacionamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criar a tabela ClinicaEspecialidades
            migrationBuilder.CreateTable(
                name: "ClinicaEspecialidades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicaId = table.Column<int>(nullable: false),
                    Especialidade = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicaEspecialidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicaEspecialidades_Clinicas_ClinicaId",
                        column: x => x.ClinicaId,
                        principalTable: "Clinicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaEspecialidades_ClinicaId",
                table: "ClinicaEspecialidades",
                column: "ClinicaId");

            // Remover a coluna ClinicaId da tabela Consultas (se existir)
            migrationBuilder.DropColumn(
                name: "ClinicaId",
                table: "Consultas");

            // Adicionar a coluna ClinicaEspecialidadeId não-nula em Consultas
            migrationBuilder.AddColumn<int>(
                name: "ClinicaEspecialidadeId",
                table: "Consultas",
                nullable: false,
                defaultValue: 0);

            // Criar FK de Consultas.ClinicaEspecialidadeId para ClinicaEspecialidades.Id
            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_ClinicaEspecialidades_ClinicaEspecialidadeId",
                table: "Consultas",
                column: "ClinicaEspecialidadeId",
                principalTable: "ClinicaEspecialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover FK e coluna ClinicaEspecialidadeId
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_ClinicaEspecialidades_ClinicaEspecialidadeId",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "ClinicaEspecialidadeId",
                table: "Consultas");

            // Recriar a coluna ClinicaId na tabela Consultas
            migrationBuilder.AddColumn<int>(
                name: "ClinicaId",
                table: "Consultas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Remover tabela ClinicaEspecialidades
            migrationBuilder.DropTable(
                name: "ClinicaEspecialidades");
        }
    }
}
