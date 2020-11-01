using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class initTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_User_UserRecipientId1",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_User_UserSenderId1",
                table: "Friendship");

            migrationBuilder.DropIndex(
                name: "IX_Friendship_UserRecipientId1",
                table: "Friendship");

            migrationBuilder.DropIndex(
                name: "IX_Friendship_UserSenderId1",
                table: "Friendship");

            migrationBuilder.DropColumn(
                name: "UserRecipientId1",
                table: "Friendship");

            migrationBuilder.DropColumn(
                name: "UserSenderId1",
                table: "Friendship");

            migrationBuilder.AlterColumn<long>(
                name: "UserSenderId",
                table: "Friendship",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "UserRecipientId",
                table: "Friendship",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "StatusCodeID",
                table: "Friendship",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_StatusCodeID",
                table: "Friendship",
                column: "StatusCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserSenderId",
                table: "Friendship",
                column: "UserSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_StatusCode",
                table: "Friendship",
                column: "StatusCodeID",
                principalTable: "StatusCode",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_User_UserRecipientId",
                table: "Friendship",
                column: "UserRecipientId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_User_UserSenderId",
                table: "Friendship",
                column: "UserSenderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_StatusCode",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_User_UserRecipientId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_User_UserSenderId",
                table: "Friendship");

            migrationBuilder.DropTable(
                name: "StatusCode");

            migrationBuilder.DropIndex(
                name: "IX_Friendship_StatusCodeID",
                table: "Friendship");

            migrationBuilder.DropIndex(
                name: "IX_Friendship_UserSenderId",
                table: "Friendship");

            migrationBuilder.DropColumn(
                name: "StatusCodeID",
                table: "Friendship");

            migrationBuilder.AlterColumn<int>(
                name: "UserSenderId",
                table: "Friendship",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "UserRecipientId",
                table: "Friendship",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "UserRecipientId1",
                table: "Friendship",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserSenderId1",
                table: "Friendship",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserRecipientId1",
                table: "Friendship",
                column: "UserRecipientId1");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserSenderId1",
                table: "Friendship",
                column: "UserSenderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_User_UserRecipientId1",
                table: "Friendship",
                column: "UserRecipientId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_User_UserSenderId1",
                table: "Friendship",
                column: "UserSenderId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
