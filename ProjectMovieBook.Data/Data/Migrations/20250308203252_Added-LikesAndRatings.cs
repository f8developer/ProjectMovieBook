using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectMovieBook.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLikesAndRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_CreatedByUserId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_CreatedByUserId",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "UserBookLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBookLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookLikes_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBookRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    RatingValue = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBookRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookRatings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMovieLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMovieLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMovieLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMovieLikes_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMovieRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    RatingValue = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMovieRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMovieRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMovieRatings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBookLikes_BookId",
                table: "UserBookLikes",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookLikes_UserId",
                table: "UserBookLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookRatings_BookId",
                table: "UserBookRatings",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookRatings_UserId",
                table: "UserBookRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieLikes_MovieId",
                table: "UserMovieLikes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieLikes_UserId",
                table: "UserMovieLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieRatings_MovieId",
                table: "UserMovieRatings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMovieRatings_UserId",
                table: "UserMovieRatings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_CreatedByUserId",
                table: "Books",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_CreatedByUserId",
                table: "Movies",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_CreatedByUserId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_CreatedByUserId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "UserBookLikes");

            migrationBuilder.DropTable(
                name: "UserBookRatings");

            migrationBuilder.DropTable(
                name: "UserMovieLikes");

            migrationBuilder.DropTable(
                name: "UserMovieRatings");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_CreatedByUserId",
                table: "Books",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_CreatedByUserId",
                table: "Movies",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
