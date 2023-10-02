using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class ADD_typed_entity_ids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update values in the PaymentSpecs.Disriminator column: BasePaymentSpec -> CashAccountSpec
            migrationBuilder.Sql("UPDATE PaymentSpecs SET Discriminator = 'CashAccountSpec' WHERE Discriminator = 'BasePaymentSpec'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the Up migration
            migrationBuilder.Sql("UPDATE PaymentSpecs SET Discriminator = 'BasePaymentSpec' WHERE Discriminator = 'CashAccountSpec'");
        }
    }
}
