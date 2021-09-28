using Microsoft.EntityFrameworkCore.Migrations;

namespace DataImporter.Web.Data.Migrations.Importing
{
    public partial class TypoFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "History",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "History",
                newName: "status");
        }
    }
}
