using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class AddCheckContraintToPhoneNumberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // It is important to make sure the existing values of the PhoneNumberType respects
            // the newly added constraint, else the following CreateCheckConstraint method will fail.
            // This is done in a more straight forward manner for demo purposes, in production environment,
            // there would probably be more logic involved to handle existing data.
            migrationBuilder.Sql("UPDATE PhoneNumber SET PhoneNumberType = NULL WHERE PhoneNumberType NOT IN ('Mobile', 'Home', 'Office')");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_PhoneNumber_PhoneNumberType",
                table: "PhoneNumber",
                sql: "PhoneNumberType IN ('Mobile', 'Home', 'Office')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_PhoneNumber_PhoneNumberType",
                table: "PhoneNumber");
        }
    }
}
