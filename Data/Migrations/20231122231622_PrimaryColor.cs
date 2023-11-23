using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoliBaza.Data.Migrations
{
    public partial class PrimaryColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrimaryColor",
                table: "UserPreferences",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryColor",
                table: "UserPreferences");
        }
    }
}
