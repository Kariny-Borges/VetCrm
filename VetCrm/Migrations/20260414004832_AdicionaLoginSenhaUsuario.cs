using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaLoginSenhaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Usuarios");
        }
    }
}
