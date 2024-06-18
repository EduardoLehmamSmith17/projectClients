using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projeto_clientes.Migrations
{
    /// <inheritdoc />
    public partial class gravandoColunas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_PessoasFisicas_PessoaFisicaId",
                table: "Contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_PessoasJuridicas_PessoaJuridicaId",
                table: "Contatos");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_PessoasFisicas_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId",
                principalTable: "PessoasFisicas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_PessoasJuridicas_PessoaJuridicaId",
                table: "Contatos",
                column: "PessoaJuridicaId",
                principalTable: "PessoasJuridicas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_PessoasFisicas_PessoaFisicaId",
                table: "Contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_PessoasJuridicas_PessoaJuridicaId",
                table: "Contatos");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_PessoasFisicas_PessoaFisicaId",
                table: "Contatos",
                column: "PessoaFisicaId",
                principalTable: "PessoasFisicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_PessoasJuridicas_PessoaJuridicaId",
                table: "Contatos",
                column: "PessoaJuridicaId",
                principalTable: "PessoasJuridicas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
