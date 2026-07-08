using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaSituacaoConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Situacao",
                table: "Consultas",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Consultas");
        }
    }
}
