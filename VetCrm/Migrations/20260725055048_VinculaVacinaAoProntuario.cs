using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class VinculaVacinaAoProntuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProntuarioId",
                table: "PacienteVacinas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PacienteVacinas_ProntuarioId",
                table: "PacienteVacinas",
                column: "ProntuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_PacienteVacinas_Prontuarios_ProntuarioId",
                table: "PacienteVacinas",
                column: "ProntuarioId",
                principalTable: "Prontuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacienteVacinas_Prontuarios_ProntuarioId",
                table: "PacienteVacinas");

            migrationBuilder.DropIndex(
                name: "IX_PacienteVacinas_ProntuarioId",
                table: "PacienteVacinas");

            migrationBuilder.DropColumn(
                name: "ProntuarioId",
                table: "PacienteVacinas");
        }
    }
}
