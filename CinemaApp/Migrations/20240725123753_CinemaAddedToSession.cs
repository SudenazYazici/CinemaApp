using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Migrations
{
    /// <inheritdoc />
    public partial class CinemaAddedToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaId",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CinemaId",
                table: "Sessions",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Cinemas_CinemaId",
                table: "Sessions",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Cinemas_CinemaId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_CinemaId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CinemaId",
                table: "Sessions");
        }
    }
}
