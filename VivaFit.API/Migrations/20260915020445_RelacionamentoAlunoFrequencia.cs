using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VivaFit.API.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoAlunoFrequencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Frequencias_AlunoId",
                table: "Frequencias",
                column: "AlunoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Frequencias_Alunos_AlunoId",
                table: "Frequencias",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Frequencias_Alunos_AlunoId",
                table: "Frequencias");

            migrationBuilder.DropIndex(
                name: "IX_Frequencias_AlunoId",
                table: "Frequencias");
        }
    }
}
