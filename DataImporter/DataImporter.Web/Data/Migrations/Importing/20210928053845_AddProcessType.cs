using Microsoft.EntityFrameworkCore.Migrations;

namespace DataImporter.Web.Data.Migrations.Importing
{
    public partial class AddProcessType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProcessType",
                table: "History",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessType",
                table: "History");
        }
    }
}
