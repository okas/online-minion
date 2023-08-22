using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.Data.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_Entity_and_member_renames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyIso",
                table: "PaymentSpecs",
                newName: "CurrencyCode");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "PaymentSpecs",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "PaymentSpecs",
                newName: "CurrencyIso");

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
    }
}
