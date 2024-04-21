using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirtimePurchase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Telcos = table.Column<string>(type: "text", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    StatusCode = table.Column<int>(type: "integer", nullable: false),
                    TransactionReference = table.Column<string>(type: "text", nullable: true),
                    APIResponse = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DebitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirtimePurchase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: true),
                    Ip = table.Column<string>(type: "text", nullable: true),
                    IsUser = table.Column<string>(type: "text", nullable: true),
                    UserIdentification = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<string>(type: "text", nullable: true),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    TransactionRef = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DebitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApiResponse = table.Column<string>(type: "text", nullable: true),
                    TransactionStatus = table.Column<string>(type: "text", nullable: true),
                    StatusCode = table.Column<string>(type: "text", nullable: true),
                    ApiUrl = table.Column<string>(type: "text", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false),
                    ReversalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CableBillsPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    TranasactionRef = table.Column<string>(type: "text", nullable: true),
                    BillType = table.Column<string>(type: "text", nullable: true),
                    ServiceProvider = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    DebitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    Package = table.Column<string>(type: "text", nullable: true),
                    APIResponse = table.Column<string>(type: "text", nullable: true),
                    ServiceUrl = table.Column<string>(type: "text", nullable: true),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false),
                    ReversalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CableBillsPayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    BVN = table.Column<string>(type: "text", nullable: true),
                    AccountType = table.Column<string>(type: "text", nullable: true),
                    IdentificationType = table.Column<string>(type: "text", nullable: true),
                    IdenitificationNumber = table.Column<string>(type: "text", nullable: true),
                    AccountNumber = table.Column<string>(type: "text", nullable: true),
                    TransactionLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    ProfileImage = table.Column<string>(type: "text", nullable: true),
                    DateOnboarded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountBalance = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTransactionTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTransactionTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataPurchase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Telcos = table.Column<string>(type: "text", nullable: true),
                    DataPlan = table.Column<string>(type: "text", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    StatusCode = table.Column<int>(type: "integer", nullable: false),
                    TransactionReference = table.Column<string>(type: "text", nullable: true),
                    APIResponse = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DebitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPurchase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherBankTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    BankCode = table.Column<int>(type: "integer", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: true),
                    TransactionRef = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DebitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ApiResponse = table.Column<string>(type: "text", nullable: true),
                    DuneTransactionReference = table.Column<string>(type: "text", nullable: true),
                    DuneApiResponse = table.Column<string>(type: "text", nullable: true),
                    TransactionStatus = table.Column<string>(type: "text", nullable: true),
                    StatusCode = table.Column<string>(type: "text", nullable: true),
                    ApiUrl = table.Column<string>(type: "text", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false),
                    ReversalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherBankTransfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UtitlityBillPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: true),
                    TranasactionRef = table.Column<string>(type: "text", nullable: true),
                    BillType = table.Column<string>(type: "text", nullable: true),
                    ServiceProvider = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    DebitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    Package = table.Column<string>(type: "text", nullable: true),
                    APIResponse = table.Column<string>(type: "text", nullable: true),
                    ServiceUrl = table.Column<string>(type: "text", nullable: true),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false),
                    ReversalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtitlityBillPayments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirtimePurchase");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "BankTransfers");

            migrationBuilder.DropTable(
                name: "CableBillsPayments");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerTransactionTokens");

            migrationBuilder.DropTable(
                name: "DataPurchase");

            migrationBuilder.DropTable(
                name: "OtherBankTransfers");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UtitlityBillPayments");
        }
    }
}
