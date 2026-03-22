using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaContatoEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Veterinarios");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Veterinarios");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Proprietarios");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Proprietarios");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Proprietarios");

            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Proprietarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Contato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProprietarioId = table.Column<int>(type: "int", nullable: true),
                    VeterinarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contato_Proprietarios_ProprietarioId",
                        column: x => x.ProprietarioId,
                        principalTable: "Proprietarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contato_Veterinarios_VeterinarioId",
                        column: x => x.VeterinarioId,
                        principalTable: "Veterinarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logradouro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CEP = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proprietarios_EnderecoId",
                table: "Proprietarios",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Contato_ProprietarioId",
                table: "Contato",
                column: "ProprietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Contato_VeterinarioId",
                table: "Contato",
                column: "VeterinarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proprietarios_Endereco_EnderecoId",
                table: "Proprietarios",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proprietarios_Endereco_EnderecoId",
                table: "Proprietarios");

            migrationBuilder.DropTable(
                name: "Contato");

            migrationBuilder.DropTable(
                name: "Endereco");

            migrationBuilder.DropIndex(
                name: "IX_Proprietarios_EnderecoId",
                table: "Proprietarios");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Proprietarios");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Veterinarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Veterinarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Proprietarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Proprietarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Proprietarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
