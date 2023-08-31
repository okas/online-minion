using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.Data.Migrations
{
    /// <inheritdoc />
    public partial class FIX_TransactionDebit_to_AccountSpec_relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionDebits_AccountSpecId",
                table: "TransactionDebits");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDebits_AccountSpecId",
                table: "TransactionDebits",
                column: "AccountSpecId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionDebits_AccountSpecId",
                table: "TransactionDebits");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDebits_AccountSpecId",
                table: "TransactionDebits",
                column: "AccountSpecId",
                unique: true,
                filter: "[AccountSpecId] IS NOT NULL");
        }
    }
}
