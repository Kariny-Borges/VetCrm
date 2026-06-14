using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class EstabelecimentoHerdaPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstabelecimentoId",
                table: "Contato",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contato_EstabelecimentoId",
                table: "Contato",
                column: "EstabelecimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contato_Estabelecimentos_EstabelecimentoId",
                table: "Contato",
                column: "EstabelecimentoId",
                principalTable: "Estabelecimentos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contato_Estabelecimentos_EstabelecimentoId",
                table: "Contato");

            migrationBuilder.DropIndex(
                name: "IX_Contato_EstabelecimentoId",
                table: "Contato");

            migrationBuilder.DropColumn(
                name: "EstabelecimentoId",
                table: "Contato");
        }
    }
}
