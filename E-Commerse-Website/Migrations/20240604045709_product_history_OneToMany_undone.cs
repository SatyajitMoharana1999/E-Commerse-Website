using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class product_history_OneToMany_undone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_adminHistory_tbl_product_pro_id",
                table: "tbl_adminHistory");

            migrationBuilder.DropIndex(
                name: "IX_tbl_adminHistory_pro_id",
                table: "tbl_adminHistory");

            migrationBuilder.DropColumn(
                name: "pro_id",
                table: "tbl_adminHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "pro_id",
                table: "tbl_adminHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_adminHistory_pro_id",
                table: "tbl_adminHistory",
                column: "pro_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_adminHistory_tbl_product_pro_id",
                table: "tbl_adminHistory",
                column: "pro_id",
                principalTable: "tbl_product",
                principalColumn: "product_id");
        }
    }
}
