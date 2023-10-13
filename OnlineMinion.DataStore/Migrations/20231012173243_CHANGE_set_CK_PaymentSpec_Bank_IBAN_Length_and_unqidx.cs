using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_set_CK_PaymentSpec_Bank_IBAN_Length_and_unqidx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaymentSpecs_IBAN",
                table: "PaymentSpecs",
                column: "IBAN",
                unique: true,
                filter: "[IBAN] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PaymentSpecBank_IBAN_Length",
                table: "PaymentSpecs",
                sql: "LEN(IBAN) BETWEEN 16 AND 34");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentSpecs_IBAN",
                table: "PaymentSpecs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PaymentSpecBank_IBAN_Length",
                table: "PaymentSpecs");
        }
    }
}
