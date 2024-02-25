using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongDe.Migrations
{
    /// <inheritdoc />
    public partial class addShopTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Shops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Shops",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_UserId",
                table: "Shops",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_AspNetUsers_UserId",
                table: "Shops",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_AspNetUsers_UserId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_UserId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Shops");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
