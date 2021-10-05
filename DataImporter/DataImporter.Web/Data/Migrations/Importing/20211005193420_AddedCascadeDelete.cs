using Microsoft.EntityFrameworkCore.Migrations;

namespace DataImporter.Web.Data.Migrations.Importing
{
    public partial class AddedCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cells_Rows_RowId",
                table: "Cells");

            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Groups_GroupId",
                table: "Columns");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Groups_GroupId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_Rows_Groups_GroupId",
                table: "Rows");

            migrationBuilder.AddForeignKey(
                name: "FK_Cells_Rows_RowId",
                table: "Cells",
                column: "RowId",
                principalTable: "Rows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Groups_GroupId",
                table: "Columns",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Groups_GroupId",
                table: "History",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rows_Groups_GroupId",
                table: "Rows",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cells_Rows_RowId",
                table: "Cells");

            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Groups_GroupId",
                table: "Columns");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Groups_GroupId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_Rows_Groups_GroupId",
                table: "Rows");

            migrationBuilder.AddForeignKey(
                name: "FK_Cells_Rows_RowId",
                table: "Cells",
                column: "RowId",
                principalTable: "Rows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Groups_GroupId",
                table: "Columns",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Groups_GroupId",
                table: "History",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rows_Groups_GroupId",
                table: "Rows",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
