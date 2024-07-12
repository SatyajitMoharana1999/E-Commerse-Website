using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class otptableupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "tbl_OTP_Customer");

            migrationBuilder.AddColumn<bool>(
                name: "isVerified",
                table: "tbl_customer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isVerified",
                table: "tbl_customer");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "tbl_OTP_Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
