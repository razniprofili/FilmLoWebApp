﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieJMDBApi",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Poster = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieJMDBApi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusCode",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCode", x => x.Code);
                });

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
                    Picture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieDetailsJMDBApi",
                columns: table => new
                {
                    MovieJMDBApiId = table.Column<string>(nullable: false),
                    Actors = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Director = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    Genre = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieDetailsJMDBApi", x => x.MovieJMDBApiId);
                    table.ForeignKey(
                        name: "FK_MovieDetailsJMDBApi_MovieJMDBApi_MovieJMDBApiId",
                        column: x => x.MovieJMDBApiId,
                        principalTable: "MovieJMDBApi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    UserSenderId = table.Column<long>(nullable: false),
                    UserRecipientId = table.Column<long>(nullable: false),
                    FriendshipDate = table.Column<DateTime>(nullable: false),
                    StatusCodeID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => new { x.UserRecipientId, x.UserSenderId });
                    table.ForeignKey(
                        name: "FK_Friendship_StatusCode_StatusCodeID",
                        column: x => x.StatusCodeID,
                        principalTable: "StatusCode",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendship_User_UserRecipientId",
                        column: x => x.UserRecipientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendship_User_UserSenderId",
                        column: x => x.UserSenderId,
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
                    MovieJMDBApiId = table.Column<string>(nullable: false),
                    WatchingDate = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedMovie", x => new { x.UserId, x.MovieJMDBApiId });
                    table.ForeignKey(
                        name: "FK_WatchedMovie_MovieJMDBApi_MovieJMDBApiId",
                        column: x => x.MovieJMDBApiId,
                        principalTable: "MovieJMDBApi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchedMovie_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_StatusCodeID",
                table: "Friendship",
                column: "StatusCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserSenderId",
                table: "Friendship",
                column: "UserSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedMovie_MovieJMDBApiId",
                table: "SavedMovie",
                column: "MovieJMDBApiId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedMovie_MovieJMDBApiId",
                table: "WatchedMovie",
                column: "MovieJMDBApiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.DropTable(
                name: "MovieDetailsJMDBApi");

            migrationBuilder.DropTable(
                name: "SavedMovie");

            migrationBuilder.DropTable(
                name: "WatchedMovie");

            migrationBuilder.DropTable(
                name: "StatusCode");

            migrationBuilder.DropTable(
                name: "MovieJMDBApi");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
