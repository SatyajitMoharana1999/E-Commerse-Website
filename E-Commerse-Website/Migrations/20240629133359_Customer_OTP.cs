using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class Customer_OTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_OTP_Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_OTP_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_OTP_Customer_tbl_customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "tbl_customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_OTP_Customer_CustomerId",
                table: "tbl_OTP_Customer",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_OTP_Customer");
        }
    }
}
