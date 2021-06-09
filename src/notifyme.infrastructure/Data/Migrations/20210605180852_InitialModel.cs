using Microsoft.EntityFrameworkCore.Migrations;

namespace notifyme.infrastructure.Data.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NotificationTitle = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    NotificationBody = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedNotificationSubscriptions",
                columns: table => new
                {
                    P256hKey = table.Column<string>(type: "TEXT", nullable: false),
                    AuthKey = table.Column<string>(type: "TEXT", nullable: true),
                    EndPoint = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedNotificationSubscriptions", x => x.P256hKey);
                    table.ForeignKey(
                        name: "FK_SavedNotificationSubscriptions_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserName",
                table: "Notifications",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_SavedNotificationSubscriptions_UserName",
                table: "SavedNotificationSubscriptions",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SavedNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
