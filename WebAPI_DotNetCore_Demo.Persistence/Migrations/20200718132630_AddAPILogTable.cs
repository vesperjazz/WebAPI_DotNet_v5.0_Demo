using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class AddAPILogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APILog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(nullable: true),
                    Level = table.Column<string>(maxLength: 20, nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    RequestMethod = table.Column<string>(maxLength: 10, nullable: false),
                    RequestPath = table.Column<string>(nullable: true),
                    RequestBody = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<int>(nullable: false),
                    ResponseBody = table.Column<string>(nullable: true),
                    ElapsedMs = table.Column<double>(nullable: true),
                    UserName = table.Column<string>(maxLength: 100, nullable: true),
                    MachineName = table.Column<string>(nullable: true),
                    ProcessId = table.Column<int>(nullable: false),
                    ThreadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APILog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APILog");
        }
    }
}
