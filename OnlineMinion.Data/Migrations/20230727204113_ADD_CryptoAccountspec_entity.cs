using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.Data.Migrations
{
    /// <inheritdoc />
    public partial class ADD_CryptoAccountspec_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExchangeName",
                table: "PaymentSpecs",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFiat",
                table: "PaymentSpecs",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeName",
                table: "PaymentSpecs");

            migrationBuilder.DropColumn(
                name: "IsFiat",
                table: "PaymentSpecs");
        }
    }
}
