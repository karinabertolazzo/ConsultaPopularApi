using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaPopularApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEspecialidadeFromClinicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Especialidade",
            table: "Clinicas");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "Especialidade",
            table: "Clinicas",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        }
    }
}
