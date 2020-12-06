using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Name_Age",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_Name_Age",
                table: "User",
                columns: new[] { "Name", "Age" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Name_Age",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_Name_Age",
                table: "User",
                columns: new[] { "Name", "Age" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
