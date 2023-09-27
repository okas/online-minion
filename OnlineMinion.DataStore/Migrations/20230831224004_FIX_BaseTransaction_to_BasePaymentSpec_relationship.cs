using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class FIX_BaseTransaction_to_BasePaymentSpec_relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionDebits_PaymentInstrumentId",
                table: "TransactionDebits");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCredits_PaymentInstrumentId",
                table: "TransactionCredits");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDebits_PaymentInstrumentId",
                table: "TransactionDebits",
                column: "PaymentInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCredits_PaymentInstrumentId",
                table: "TransactionCredits",
                column: "PaymentInstrumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionDebits_PaymentInstrumentId",
                table: "TransactionDebits");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCredits_PaymentInstrumentId",
                table: "TransactionCredits");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDebits_PaymentInstrumentId",
                table: "TransactionDebits",
                column: "PaymentInstrumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCredits_PaymentInstrumentId",
                table: "TransactionCredits",
                column: "PaymentInstrumentId",
                unique: true);
        }
    }
}
