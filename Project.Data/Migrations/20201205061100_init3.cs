using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Name",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "Index_Name",
                table: "User",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Name",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "Index_Name",
                table: "User",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
