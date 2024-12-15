using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class postgreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_admin",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    admin_name = table.Column<string>(type: "text", nullable: false),
                    admin_email = table.Column<string>(type: "text", nullable: false),
                    admin_password = table.Column<string>(type: "text", nullable: true),
                    admin_image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_admin", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_name = table.Column<string>(type: "text", nullable: true),
                    category_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_customer",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_name = table.Column<string>(type: "text", nullable: true),
                    customer_phone = table.Column<string>(type: "text", nullable: true),
                    customer_email = table.Column<string>(type: "text", nullable: true),
                    customer_password = table.Column<string>(type: "text", nullable: true),
                    customer_gender = table.Column<string>(type: "text", nullable: true),
                    customer_country = table.Column<string>(type: "text", nullable: true),
                    customer_city = table.Column<string>(type: "text", nullable: true),
                    customer_address = table.Column<string>(type: "text", nullable: true),
                    isVerified = table.Column<bool>(type: "boolean", nullable: false),
                    customer_image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_customer", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_faqs",
                columns: table => new
                {
                    faq_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    faq_question = table.Column<string>(type: "text", nullable: false),
                    faq_answer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_faqs", x => x.faq_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_feedback",
                columns: table => new
                {
                    feedback_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    feedback_name = table.Column<string>(type: "text", nullable: false),
                    feedback_message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_feedback", x => x.feedback_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Section",
                columns: table => new
                {
                    section_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    section_name = table.Column<string>(type: "text", nullable: true),
                    section_description = table.Column<string>(type: "text", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    section_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Section", x => x.section_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Slider",
                columns: table => new
                {
                    slider_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    slider_name = table.Column<string>(type: "text", nullable: true),
                    slider_description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    slider_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Slider", x => x.slider_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_adminHistory",
                columns: table => new
                {
                    AH_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AH_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AH_title = table.Column<string>(type: "text", nullable: true),
                    AH_description = table.Column<string>(type: "text", nullable: true),
                    AH_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    admin_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_adminHistory", x => x.AH_id);
                    table.ForeignKey(
                        name: "FK_tbl_adminHistory_tbl_admin_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_admin",
                        principalColumn: "admin_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    product_price = table.Column<string>(type: "text", nullable: false),
                    product_description = table.Column<string>(type: "text", nullable: false),
                    product_image = table.Column<string>(type: "text", nullable: true),
                    product_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    cat_id = table.Column<int>(type: "integer", nullable: false),
                    adm_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_tbl_product_tbl_admin_adm_id",
                        column: x => x.adm_id,
                        principalTable: "tbl_admin",
                        principalColumn: "admin_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_product_tbl_category_cat_id",
                        column: x => x.cat_id,
                        principalTable: "tbl_category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_OTP_Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "tbl_cart",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prod_id = table.Column<int>(type: "integer", nullable: false),
                    cust_id = table.Column<int>(type: "integer", nullable: false),
                    product_quantity = table.Column<int>(type: "integer", nullable: false),
                    cart_status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cart", x => x.cart_id);
                    table.ForeignKey(
                        name: "FK_tbl_cart_tbl_product_prod_id",
                        column: x => x.prod_id,
                        principalTable: "tbl_product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SectionProduct",
                columns: table => new
                {
                    section_product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false),
                    section_product_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    section_id = table.Column<int>(type: "integer", nullable: false),
                    prod_id = table.Column<int>(type: "integer", nullable: false)
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
                    slider_product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false),
                    slider_product_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    product_image = table.Column<string>(type: "text", nullable: true),
                    slider_id = table.Column<int>(type: "integer", nullable: false),
                    prod_id = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_tbl_adminHistory_admin_id",
                table: "tbl_adminHistory",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cart_prod_id",
                table: "tbl_cart",
                column: "prod_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_OTP_Customer_CustomerId",
                table: "tbl_OTP_Customer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_adm_id",
                table: "tbl_product",
                column: "adm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_product_cat_id",
                table: "tbl_product",
                column: "cat_id");

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
                name: "tbl_adminHistory");

            migrationBuilder.DropTable(
                name: "tbl_cart");

            migrationBuilder.DropTable(
                name: "tbl_faqs");

            migrationBuilder.DropTable(
                name: "tbl_feedback");

            migrationBuilder.DropTable(
                name: "tbl_OTP_Customer");

            migrationBuilder.DropTable(
                name: "tbl_SectionProduct");

            migrationBuilder.DropTable(
                name: "tbl_SliderProduct");

            migrationBuilder.DropTable(
                name: "tbl_customer");

            migrationBuilder.DropTable(
                name: "tbl_Section");

            migrationBuilder.DropTable(
                name: "tbl_Slider");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_admin");

            migrationBuilder.DropTable(
                name: "tbl_category");
        }
    }
}
