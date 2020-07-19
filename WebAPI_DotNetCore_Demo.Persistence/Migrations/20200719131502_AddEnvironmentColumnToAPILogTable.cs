using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class AddEnvironmentColumnToAPILogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Environment",
                table: "APILog",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Environment",
                table: "APILog");
        }
    }
}
