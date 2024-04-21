using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USSDDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddedDOBUSSD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Customers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Customers",
                type: "text",
                nullable: true);
        }
    }
}
