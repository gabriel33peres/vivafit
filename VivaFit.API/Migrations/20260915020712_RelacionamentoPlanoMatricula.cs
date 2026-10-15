using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VivaFit.API.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoPlanoMatricula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Matriculas_PlanoId",
                table: "Matriculas",
                column: "PlanoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Planos_PlanoId",
                table: "Matriculas",
                column: "PlanoId",
                principalTable: "Planos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Planos_PlanoId",
                table: "Matriculas");

            migrationBuilder.DropIndex(
                name: "IX_Matriculas_PlanoId",
                table: "Matriculas");
        }
    }
}
