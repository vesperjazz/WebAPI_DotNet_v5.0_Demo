using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class RemoveUserNameAlternateKeyInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_UserName",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_UserName",
                table: "User");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_UserName",
                table: "User",
                column: "UserName");
        }
    }
}
