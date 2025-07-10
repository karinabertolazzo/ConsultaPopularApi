using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaPopularApi.Migrations
{
    /// <inheritdoc />
    public partial class AjusteEnderecoEEspecialidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Clinicas",
                newName: "Telefone");

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Bairro",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Cep",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Cidade",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Complemento",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Estado",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Numero",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco_Rua",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HorarioFuncionamento",
                table: "Clinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Endereco_Bairro",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Endereco_Cep",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Endereco_Cidade",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Endereco_Complemento",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Endereco_Estado",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Endereco_Numero",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "Endereco_Rua",
                table: "Clinicas");

            migrationBuilder.DropColumn(
                name: "HorarioFuncionamento",
                table: "Clinicas");

            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "Clinicas",
                newName: "Endereco");
        }
    }
}
