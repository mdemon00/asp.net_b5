using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataImporter.Web.Data.Migrations.Importing
{
    public partial class AddApplicationIdInHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "History",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_History_ApplicationUserId",
                table: "History",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_AspNetUsers_ApplicationUserId",
                table: "History",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_AspNetUsers_ApplicationUserId",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_ApplicationUserId",
                table: "History");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "History");
        }
    }
}
