using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class admin_product_fk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "adm_id",
                table: "tbl_product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_adm_id",
                table: "tbl_product",
                column: "adm_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_product_tbl_admin_adm_id",
                table: "tbl_product",
                column: "adm_id",
                principalTable: "tbl_admin",
                principalColumn: "admin_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_product_tbl_admin_adm_id",
                table: "tbl_product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_product_adm_id",
                table: "tbl_product");

            migrationBuilder.DropColumn(
                name: "adm_id",
                table: "tbl_product");
        }
    }
}
