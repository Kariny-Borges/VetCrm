using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioPerfilViraTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Perfis",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Recepção" },
                    { 2, "Administrativo" },
                    { 3, "Financeiro" },
                    { 4, "Gerente" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Perfil",
                table: "Usuarios",
                column: "Perfil");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Perfis_Perfil",
                table: "Usuarios",
                column: "Perfil",
                principalTable: "Perfis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Perfis_Perfil",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Perfil",
                table: "Usuarios");
        }
    }
}
