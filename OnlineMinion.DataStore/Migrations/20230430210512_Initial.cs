using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSpecs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrencyIso = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSpecs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCredits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Party = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentInstrumentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCredits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId",
                        column: x => x.PaymentInstrumentId,
                        principalTable: "PaymentSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDebits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Party = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentInstrumentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AccountSpecId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDebits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionDebits_AccountSpecs_AccountSpecId",
                        column: x => x.AccountSpecId,
                        principalTable: "AccountSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId",
                        column: x => x.PaymentInstrumentId,
                        principalTable: "PaymentSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountSpecs_Name",
                table: "AccountSpecs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCredits_PaymentInstrumentId",
                table: "TransactionCredits",
                column: "PaymentInstrumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDebits_AccountSpecId",
                table: "TransactionDebits",
                column: "AccountSpecId",
                unique: true,
                filter: "[AccountSpecId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDebits_PaymentInstrumentId",
                table: "TransactionDebits",
                column: "PaymentInstrumentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionCredits");

            migrationBuilder.DropTable(
                name: "TransactionDebits");

            migrationBuilder.DropTable(
                name: "AccountSpecs");

            migrationBuilder.DropTable(
                name: "PaymentSpecs");
        }
    }
}
