using Microsoft.EntityFrameworkCore.Migrations;

namespace DataImporter.Web.Data.Migrations.Importing
{
    public partial class AddEmailSentInHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailSent",
                table: "History",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSent",
                table: "History");
        }
    }
}
