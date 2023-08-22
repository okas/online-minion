using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.Data.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_BasePaymentSpec_entityconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "PaymentSpecs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSpecs_Name",
                table: "PaymentSpecs",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentSpecs_Name",
                table: "PaymentSpecs");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "PaymentSpecs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }
    }
}
