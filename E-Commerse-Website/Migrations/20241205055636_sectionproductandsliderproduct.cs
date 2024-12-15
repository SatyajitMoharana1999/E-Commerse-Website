using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class sectionproductandsliderproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "slider_product_deleted",
                table: "tbl_SliderProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "section_product_deleted",
                table: "tbl_SectionProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "slider_product_deleted",
                table: "tbl_SliderProduct");

            migrationBuilder.DropColumn(
                name: "section_product_deleted",
                table: "tbl_SectionProduct");
        }
    }
}
