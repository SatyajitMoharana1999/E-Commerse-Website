using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class foreignkeyCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tbl_cart_prod_id",
                table: "tbl_cart",
                column: "prod_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cart_tbl_product_prod_id",
                table: "tbl_cart",
                column: "prod_id",
                principalTable: "tbl_product",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cart_tbl_product_prod_id",
                table: "tbl_cart");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cart_prod_id",
                table: "tbl_cart");
        }
    }
}
