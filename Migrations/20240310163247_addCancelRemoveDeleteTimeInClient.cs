using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongDe.Migrations
{
    /// <inheritdoc />
    public partial class addCancelRemoveDeleteTimeInClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Clients");

            migrationBuilder.AddColumn<bool>(
                name: "Cancel",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancel",
                table: "Clients");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Clients",
                type: "datetime2",
                nullable: true);
        }
    }
}
