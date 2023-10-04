using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_renamed_type_PaymentSpecBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update values in PaymentSpecs.Discriminator: BankAccountSpec -> PaymentSpecBank
            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'PaymentSpecBank'
                WHERE Discriminator = 'BankAccountSpec'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the Up migration
            migrationBuilder.Sql(@"
                UPDATE PaymentSpecs
                SET Discriminator = 'BankAccountSpec'
                WHERE Discriminator = 'PaymentSpecBank'
            ");
        }
    }
}
