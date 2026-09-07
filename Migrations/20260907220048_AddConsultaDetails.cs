using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UVVConsultas.Migrations
{
    /// <inheritdoc />
    public partial class AddConsultaDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Medico",
                table: "Consultas",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "Não informado");

            migrationBuilder.AddColumn<string>(
                name: "TipoAtendimento",
                table: "Consultas",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "Consulta inicial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Medico",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "TipoAtendimento",
                table: "Consultas");
        }
    }
}
