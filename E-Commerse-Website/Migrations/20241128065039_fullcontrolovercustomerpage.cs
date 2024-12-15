using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class fullcontrolovercustomerpage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Section",
                columns: table => new
                {
                    section_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    section_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    section_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Section", x => x.section_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Slider",
                columns: table => new
                {
                    slider_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    slider_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    slider_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Slider", x => x.slider_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SectionProduct",
                columns: table => new
                {
                    section_product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false),
                    section_id = table.Column<int>(type: "int", nullable: false),
                    prod_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SectionProduct", x => x.section_product_id);
                    table.ForeignKey(
                        name: "FK_tbl_SectionProduct_tbl_Section_section_id",
                        column: x => x.section_id,
                        principalTable: "tbl_Section",
                        principalColumn: "section_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_SectionProduct_tbl_product_prod_id",
                        column: x => x.prod_id,
                        principalTable: "tbl_product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SliderProduct",
                columns: table => new
                {
                    slider_product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false),
                    product_image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    slider_id = table.Column<int>(type: "int", nullable: false),
                    prod_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SliderProduct", x => x.slider_product_id);
                    table.ForeignKey(
                        name: "FK_tbl_SliderProduct_tbl_Slider_slider_id",
                        column: x => x.slider_id,
                        principalTable: "tbl_Slider",
                        principalColumn: "slider_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_SliderProduct_tbl_product_prod_id",
                        column: x => x.prod_id,
                        principalTable: "tbl_product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SectionProduct_prod_id",
                table: "tbl_SectionProduct",
                column: "prod_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SectionProduct_section_id",
                table: "tbl_SectionProduct",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SliderProduct_prod_id",
                table: "tbl_SliderProduct",
                column: "prod_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_SliderProduct_slider_id",
                table: "tbl_SliderProduct",
                column: "slider_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_SectionProduct");

            migrationBuilder.DropTable(
                name: "tbl_SliderProduct");

            migrationBuilder.DropTable(
                name: "tbl_Section");

            migrationBuilder.DropTable(
                name: "tbl_Slider");
        }
    }
}
