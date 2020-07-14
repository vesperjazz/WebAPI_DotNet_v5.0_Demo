using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class MakeCountryIDColumnRequiredInPhoneNumberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Set existing null CountryID to SG to enable the non-nullable constraints.
            migrationBuilder.Sql("UPDATE PhoneNumber SET CountryID = 'B3F8D93A-630F-4B3F-8F43-5BA0A64B79D1' WHERE CountryID IS NULL");

            migrationBuilder.AlterColumn<Guid>(
                name: "CountryID",
                table: "PhoneNumber",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CountryID",
                table: "PhoneNumber",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
