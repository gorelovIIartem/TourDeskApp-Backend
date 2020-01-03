 using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class migr3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "UserProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "UserProfile");
        }
    }
}
