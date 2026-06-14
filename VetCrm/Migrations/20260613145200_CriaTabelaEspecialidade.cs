using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class CriaTabelaEspecialidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Veterinarios");

            migrationBuilder.AddColumn<int>(
                name: "EspecialidadeId",
                table: "Veterinarios",
                type: "int",
                nullable: true);

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
                name: "IX_Veterinarios_EspecialidadeId",
                table: "Veterinarios",
                column: "EspecialidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Veterinarios_Especialidades_EspecialidadeId",
                table: "Veterinarios",
                column: "EspecialidadeId",
                principalTable: "Especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veterinarios_Especialidades_EspecialidadeId",
                table: "Veterinarios");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropIndex(
                name: "IX_Veterinarios_EspecialidadeId",
                table: "Veterinarios");

            migrationBuilder.DropColumn(
                name: "EspecialidadeId",
                table: "Veterinarios");

            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                table: "Veterinarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
