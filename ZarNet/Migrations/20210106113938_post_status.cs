using Microsoft.EntityFrameworkCore.Migrations;

namespace ZarNet.Migrations
{
    public partial class post_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Post",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Post",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Post",
                newName: "Name");
        }
    }
}
