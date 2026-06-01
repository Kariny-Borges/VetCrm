using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCrm.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaHerancaPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cria a tabela Contato que nunca havia sido criada no banco
            // (o snapshot do EF ja a esperava, mas a migration de criacao ficou para tras)
            migrationBuilder.CreateTable(
                name: "Contato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProprietarioId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Contato_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contato_Veterinarios_VeterinarioId",
                        column: x => x.VeterinarioId,
                        principalTable: "Veterinarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contato_ProprietarioId",
                table: "Contato",
                column: "ProprietarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Contato_UsuarioId",
                table: "Contato",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Contato_VeterinarioId",
                table: "Contato",
                column: "VeterinarioId");

            // Beneficio da heranca: Veterinario agora reaproveita o Endereco da classe Pessoa
            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Veterinarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veterinarios_EnderecoId",
                table: "Veterinarios",
                column: "EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Veterinarios_Enderecos_EnderecoId",
                table: "Veterinarios",
                column: "EnderecoId",
                principalTable: "Enderecos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove a tabela Contato inteira (e, junto, todas as suas FKs e indices)
            migrationBuilder.DropTable(
                name: "Contato");

            migrationBuilder.DropForeignKey(
                name: "FK_Veterinarios_Enderecos_EnderecoId",
                table: "Veterinarios");

            migrationBuilder.DropIndex(
                name: "IX_Veterinarios_EnderecoId",
                table: "Veterinarios");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Veterinarios");
        }
    }
}
