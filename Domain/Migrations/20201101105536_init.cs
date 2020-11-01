using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    MovieDetailsJMDBApiId = table.Column<long>(nullable: true),
                    MovieJMDBApiId = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    UserSenderId = table.Column<int>(nullable: false),
                    UserRecipientId = table.Column<int>(nullable: false),
                    UserSenderId1 = table.Column<long>(nullable: true),
                    UserRecipientId1 = table.Column<long>(nullable: true),
                    FriendshipDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => new { x.UserRecipientId, x.UserSenderId });
                    table.ForeignKey(
                        name: "FK_Friendship_User_UserRecipientId1",
                        column: x => x.UserRecipientId1,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendship_User_UserSenderId1",
                        column: x => x.UserSenderId1,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovieDetailsJMDBApi",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Actors = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Director = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    Genre = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDetailsJMDBApi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieDetailsJMDBApi_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovieJMDBApi",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Poster = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieJMDBApi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieJMDBApi_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedMovie",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    MovieJMDBApiId = table.Column<string>(nullable: false),
                    SavingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedMovie", x => new { x.UserId, x.MovieJMDBApiId });
                    table.ForeignKey(
                        name: "FK_SavedMovie_MovieJMDBApi_MovieJMDBApiId",
                        column: x => x.MovieJMDBApiId,
                        principalTable: "MovieJMDBApi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedMovie_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchedMovie",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    MovieDetailsJMDBApiId = table.Column<long>(nullable: false),
                    MovieDetailsJMDBApiId1 = table.Column<string>(nullable: true),
                    WatchingDate = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedMovie", x => new { x.UserId, x.MovieDetailsJMDBApiId });
                    table.ForeignKey(
                        name: "FK_WatchedMovie_MovieJMDBApi_MovieDetailsJMDBApiId1",
                        column: x => x.MovieDetailsJMDBApiId1,
                        principalTable: "MovieJMDBApi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WatchedMovie_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserRecipientId1",
                table: "Friendship",
                column: "UserRecipientId1");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserSenderId1",
                table: "Friendship",
                column: "UserSenderId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieDetailsJMDBApi_UserId",
                table: "MovieDetailsJMDBApi",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieJMDBApi_UserId",
                table: "MovieJMDBApi",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedMovie_MovieJMDBApiId",
                table: "SavedMovie",
                column: "MovieJMDBApiId");

            migrationBuilder.CreateIndex(
                name: "IX_User_MovieDetailsJMDBApiId",
                table: "User",
                column: "MovieDetailsJMDBApiId");

            migrationBuilder.CreateIndex(
                name: "IX_User_MovieJMDBApiId",
                table: "User",
                column: "MovieJMDBApiId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserId",
                table: "User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedMovie_MovieDetailsJMDBApiId1",
                table: "WatchedMovie",
                column: "MovieDetailsJMDBApiId1");

            migrationBuilder.AddForeignKey(
                name: "FK_User_MovieJMDBApi_MovieJMDBApiId",
                table: "User",
                column: "MovieJMDBApiId",
                principalTable: "MovieJMDBApi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_MovieDetailsJMDBApi_MovieDetailsJMDBApiId",
                table: "User",
                column: "MovieDetailsJMDBApiId",
                principalTable: "MovieDetailsJMDBApi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieDetailsJMDBApi_User_UserId",
                table: "MovieDetailsJMDBApi");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieJMDBApi_User_UserId",
                table: "MovieJMDBApi");

            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.DropTable(
                name: "SavedMovie");

            migrationBuilder.DropTable(
                name: "WatchedMovie");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "MovieDetailsJMDBApi");

            migrationBuilder.DropTable(
                name: "MovieJMDBApi");
        }
    }
}
