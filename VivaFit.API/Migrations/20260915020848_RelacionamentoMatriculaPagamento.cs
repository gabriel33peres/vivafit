using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VivaFit.API.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoMatriculaPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_MatriculaId",
                table: "Pagamentos",
                column: "MatriculaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Matriculas_MatriculaId",
                table: "Pagamentos",
                column: "MatriculaId",
                principalTable: "Matriculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Matriculas_MatriculaId",
                table: "Pagamentos");

            migrationBuilder.DropIndex(
                name: "IX_Pagamentos_MatriculaId",
                table: "Pagamentos");
        }
    }
}
