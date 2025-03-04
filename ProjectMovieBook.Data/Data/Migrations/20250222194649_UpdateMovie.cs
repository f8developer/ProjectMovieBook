using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMovieBook.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Directors_AuthorId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Movies",
                newName: "DirectorId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_AuthorId",
                table: "Movies",
                newName: "IX_Movies_DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Directors_DirectorId",
                table: "Movies",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Directors_DirectorId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "DirectorId",
                table: "Movies",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_DirectorId",
                table: "Movies",
                newName: "IX_Movies_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Directors_AuthorId",
                table: "Movies",
                column: "AuthorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
