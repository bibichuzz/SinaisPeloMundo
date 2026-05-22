using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SinaisPeloMundo.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pacotes_PassagemId",
                table: "Pacotes");

            migrationBuilder.DropIndex(
                name: "IX_Pacotes_ReservaHotelId",
                table: "Pacotes");

            migrationBuilder.DropColumn(
                name: "QuartoClientes",
                table: "Hoteis");

            migrationBuilder.CreateIndex(
                name: "IX_Pacotes_PassagemId",
                table: "Pacotes",
                column: "PassagemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacotes_ReservaHotelId",
                table: "Pacotes",
                column: "ReservaHotelId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pacotes_PassagemId",
                table: "Pacotes");

            migrationBuilder.DropIndex(
                name: "IX_Pacotes_ReservaHotelId",
                table: "Pacotes");

            migrationBuilder.AddColumn<int>(
                name: "QuartoClientes",
                table: "Hoteis",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pacotes_PassagemId",
                table: "Pacotes",
                column: "PassagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Pacotes_ReservaHotelId",
                table: "Pacotes",
                column: "ReservaHotelId");
        }
    }
}
