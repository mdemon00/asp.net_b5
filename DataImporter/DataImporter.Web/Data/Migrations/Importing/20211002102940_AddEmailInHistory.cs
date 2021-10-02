using Microsoft.EntityFrameworkCore.Migrations;

namespace DataImporter.Web.Data.Migrations.Importing
{
    public partial class AddEmailInHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "History",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "History");
        }
    }
}
