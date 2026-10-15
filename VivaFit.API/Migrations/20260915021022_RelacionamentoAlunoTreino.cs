using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VivaFit.API.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoAlunoTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Treinos_AlunoId",
                table: "Treinos",
                column: "AlunoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_Alunos_AlunoId",
                table: "Treinos",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_Alunos_AlunoId",
                table: "Treinos");

            migrationBuilder.DropIndex(
                name: "IX_Treinos_AlunoId",
                table: "Treinos");
        }
    }
}
