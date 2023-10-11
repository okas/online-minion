using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_PaymentSpecs_discriminator_value_shortening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update values in PaymentSpecs.Discriminator: PaymentSpecCash -> Cash, PaymentSpecBank -> Bank, PaymentSpecCrypto -> Crypto
            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'Cash'
                WHERE Discriminator = 'PaymentSpecCash'
            ");

            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'Bank'
                WHERE Discriminator = 'PaymentSpecBank'
            ");

            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'Crypto'
                WHERE Discriminator = 'PaymentSpecCrypto'
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "PaymentSpecs",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "PaymentSpecs",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6
            );

            // Reverse the Up migration value update
            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'PaymentSpecCash'
                WHERE Discriminator = 'Cash'
            ");

            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'PaymentSpecBank'
                WHERE Discriminator = 'Bank'
            ");

            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'PaymentSpecCrypto'
                WHERE Discriminator = 'Crypto'

            ");
        }
    }
}
