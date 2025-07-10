using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaPopularApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEspecialidadeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Consultas_Clinicas_ClinicaId",
            //    table: "Consultas");

            //migrationBuilder.DropIndex(
            //    name: "IX_Consultas_ClinicaId",
            //    table: "Consultas");

            //migrationBuilder.DropColumn(
            //    name: "ClinicaId",
            //    table: "Consultas");

            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "ClinicaEspecialidades");

            migrationBuilder.AddColumn<int>(
                name: "EspecialidadeId",
                table: "ClinicaEspecialidades",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicaEspecialidades_EspecialidadeId",
                table: "ClinicaEspecialidades",
                column: "EspecialidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicaEspecialidades_Especialidades_EspecialidadeId",
                table: "ClinicaEspecialidades",
                column: "EspecialidadeId",
                principalTable: "Especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicaEspecialidades_Especialidades_EspecialidadeId",
                table: "ClinicaEspecialidades");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropIndex(
                name: "IX_ClinicaEspecialidades_EspecialidadeId",
                table: "ClinicaEspecialidades");

            migrationBuilder.DropColumn(
                name: "EspecialidadeId",
                table: "ClinicaEspecialidades");

            migrationBuilder.AddColumn<int>(
                name: "ClinicaId",
                table: "Consultas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                table: "ClinicaEspecialidades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_ClinicaId",
                table: "Consultas",
                column: "ClinicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Clinicas_ClinicaId",
                table: "Consultas",
                column: "ClinicaId",
                principalTable: "Clinicas",
                principalColumn: "Id");
        }
    }
}
