using Microsoft.EntityFrameworkCore.Migrations;

namespace ZarNet.Migrations
{
    public partial class sampletreelist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "industry",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "industry",
                table: "Post");
        }
    }
}
