using Microsoft.EntityFrameworkCore.Migrations;

namespace notifyme.infrastructure.Data.Migrations
{
    public partial class addSubName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "SavedNotificationSubscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "SavedNotificationSubscriptions");
        }
    }
}
