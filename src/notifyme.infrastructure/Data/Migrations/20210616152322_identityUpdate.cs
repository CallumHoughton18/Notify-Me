using Microsoft.EntityFrameworkCore.Migrations;

namespace notifyme.infrastructure.Data.Migrations
{
    public partial class identityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_User_UserName",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedNotificationSubscriptions_User_UserName",
                table: "SavedNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_SavedNotificationSubscriptions_UserName",
                table: "SavedNotificationSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserName",
                table: "Notifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedNotificationSubscriptions_UserName",
                table: "SavedNotificationSubscriptions",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserName",
                table: "Notifications",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_User_UserName",
                table: "Notifications",
                column: "UserName",
                principalTable: "User",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedNotificationSubscriptions_User_UserName",
                table: "SavedNotificationSubscriptions",
                column: "UserName",
                principalTable: "User",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
