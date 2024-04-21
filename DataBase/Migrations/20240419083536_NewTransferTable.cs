using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class NewTransferTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Customers",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Customers",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DebitDate",
                table: "BankTransfers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "BankTransfers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryAccountName",
                table: "BankTransfers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryAccountNo",
                table: "BankTransfers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Narration",
                table: "BankTransfers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlatForm",
                table: "BankTransfers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "BankTransfers");

            migrationBuilder.DropColumn(
                name: "BeneficiaryAccountName",
                table: "BankTransfers");

            migrationBuilder.DropColumn(
                name: "BeneficiaryAccountNo",
                table: "BankTransfers");

            migrationBuilder.DropColumn(
                name: "Narration",
                table: "BankTransfers");

            migrationBuilder.DropColumn(
                name: "PlatForm",
                table: "BankTransfers");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLocked",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DebitDate",
                table: "BankTransfers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
