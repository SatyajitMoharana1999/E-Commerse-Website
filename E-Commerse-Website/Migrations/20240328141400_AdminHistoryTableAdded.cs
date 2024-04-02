using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class AdminHistoryTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_adminHistory",
                columns: table => new
                {
                    AH_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AH_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AH_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admin_id = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_tbl_adminHistory_admin_id",
                table: "tbl_adminHistory",
                column: "admin_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_adminHistory");
        }
    }
}
