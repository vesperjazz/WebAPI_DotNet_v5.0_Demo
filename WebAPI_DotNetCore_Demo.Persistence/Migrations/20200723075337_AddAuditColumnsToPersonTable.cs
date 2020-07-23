using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class AddAuditColumnsToPersonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Person",
                nullable: false,
                defaultValue: DateTime.Now);

            // Note the defaultValue of the UserID must be an existing UserID in the database.
            // DefaultValue needs to be provided due to adding new non-nullable columns to the
            // dbo.Person table.
            migrationBuilder.AddColumn<Guid>(
                name: "CreateByUserID",
                table: "Person",
                nullable: false,
                defaultValue: new Guid("9338B511-C135-41A9-9ACE-48211DB19BE9"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Person",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdateByUserID",
                table: "Person",
                nullable: false,
                defaultValue: new Guid("9338B511-C135-41A9-9ACE-48211DB19BE9"));

            migrationBuilder.CreateIndex(
                name: "IX_Person_CreateByUserID",
                table: "Person",
                column: "CreateByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_UpdateByUserID",
                table: "Person",
                column: "UpdateByUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_CreateByPersons",
                table: "Person",
                column: "CreateByUserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_UpdateByPersons",
                table: "Person",
                column: "UpdateByUserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_CreateByPersons",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_UpdateByPersons",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_CreateByUserID",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_UpdateByUserID",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "CreateByUserID",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "UpdateByUserID",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Person");
        }
    }
}
