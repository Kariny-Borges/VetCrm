using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class CriaTabelaTipoConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoConsulta",
                table: "Consultas",
                newName: "TipoConsultaId");

            migrationBuilder.CreateTable(
                name: "TiposConsulta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposConsulta", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TiposConsulta",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Agendada" },
                    { 2, "Emergência" },
                    { 3, "Retorno" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_TipoConsultaId",
                table: "Consultas",
                column: "TipoConsultaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_TiposConsulta_TipoConsultaId",
                table: "Consultas",
                column: "TipoConsultaId",
                principalTable: "TiposConsulta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_TiposConsulta_TipoConsultaId",
                table: "Consultas");

            migrationBuilder.DropTable(
                name: "TiposConsulta");

            migrationBuilder.DropIndex(
                name: "IX_Consultas_TipoConsultaId",
                table: "Consultas");

            migrationBuilder.RenameColumn(
                name: "TipoConsultaId",
                table: "Consultas",
                newName: "TipoConsulta");
        }
    }
}
