using Microsoft.EntityFrameworkCore.Migrations;

namespace notifyme.infrastructure.Data.Migrations
{
    public partial class cleanedUpDataImp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserName",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedNotificationSubscriptions_Users_UserName",
                table: "SavedNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "SavedNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "P256hKey",
                table: "SavedNotificationSubscriptions",
                newName: "P256HKey");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "SavedNotificationSubscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EndPoint",
                table: "SavedNotificationSubscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthKey",
                table: "SavedNotificationSubscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Notifications",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CronJobString",
                table: "Notifications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserName",
                table: "Notifications",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedNotificationSubscriptions_Users_UserName",
                table: "SavedNotificationSubscriptions",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserName",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedNotificationSubscriptions_Users_UserName",
                table: "SavedNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "CronJobString",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "P256HKey",
                table: "SavedNotificationSubscriptions",
                newName: "P256hKey");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "SavedNotificationSubscriptions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "EndPoint",
                table: "SavedNotificationSubscriptions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AuthKey",
                table: "SavedNotificationSubscriptions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "SavedNotificationSubscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Notifications",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserName",
                table: "Notifications",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedNotificationSubscriptions_Users_UserName",
                table: "SavedNotificationSubscriptions",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
