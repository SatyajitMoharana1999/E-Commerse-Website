using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerse_Website.Migrations
{
    /// <inheritdoc />
    public partial class UndoColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AH_irreversible",
                table: "tbl_adminHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AH_irreversible",
                table: "tbl_adminHistory");
        }
    }
}
