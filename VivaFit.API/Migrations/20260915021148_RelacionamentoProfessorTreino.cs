using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VivaFit.API.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoProfessorTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Treinos_ProfessorId",
                table: "Treinos",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_Professores_ProfessorId",
                table: "Treinos",
                column: "ProfessorId",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_Professores_ProfessorId",
                table: "Treinos");

            migrationBuilder.DropIndex(
                name: "IX_Treinos_ProfessorId",
                table: "Treinos");
        }
    }
}
