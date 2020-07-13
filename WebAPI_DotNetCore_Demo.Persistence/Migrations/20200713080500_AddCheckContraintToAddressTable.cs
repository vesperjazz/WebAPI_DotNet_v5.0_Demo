using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI_DotNetCore_Demo.Persistence.Migrations
{
    public partial class AddCheckContraintToAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Refer to comments on AddCheckContraintToPhoneNumberTable migration.
            migrationBuilder.Sql("UPDATE Address SET AddressType = NULL WHERE AddressType NOT IN ('Home', 'Office')");

            migrationBuilder.CreateCheckConstraint(
                name: "CK_Address_AddressType",
                table: "Address",
                sql: "AddressType IN ('Home', 'Office')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Address_AddressType",
                table: "Address");
        }
    }
}
