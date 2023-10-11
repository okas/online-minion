using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_renamed_type_PaymentSpecCrypto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update values in PaymentSpecs.Discriminator: CryptoExchangeAccountSpec -> PaymentSpecCrypto
            migrationBuilder.Sql(
                @"
                UPDATE PaymentSpecs
                SET Discriminator = 'PaymentSpecCrypto'
                WHERE Discriminator = 'CryptoExchangeAccountSpec'
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "PaymentSpecs",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(34)",
                oldMaxLength: 34);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "PaymentSpecs",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            // Reverse the Up migration value update
            migrationBuilder.Sql(
                @"
                UPDATE PaymentSpecs
                SET Discriminator = 'CryptoExchangeAccountSpec'
                WHERE Discriminator = 'PaymentSpecCrypto'
            ");
        }
    }
}
