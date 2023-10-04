using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_renamed_type_PaymentSpecCash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update values in PaymentSpecs.Discriminator: CashAccountSpec -> PaymentSpecCash
            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'PaymentSpecCash'
                WHERE Discriminator = 'CashAccountSpec'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the Up migration
            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'CashAccountSpec'
                WHERE Discriminator = 'PaymentSpecCash'
            ");
        }
    }
}
