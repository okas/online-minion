using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class Add_HiLobased_PK_generation : Migration
    {
        /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10
            );

            migrationBuilder.DropForeignKey("FK_TransactionDebits_AccountSpecs_AccountSpecId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId", "TransactionCredits");

            migrationBuilder.DropPrimaryKey("PK_TransactionDebits", "TransactionDebits");
            migrationBuilder.DropPrimaryKey("PK_TransactionCredits", "TransactionCredits");
            migrationBuilder.DropPrimaryKey("PK_PaymentSpecs", "PaymentSpecs");
            migrationBuilder.DropPrimaryKey("PK_AccountSpecs", "AccountSpecs");

            migrationBuilder.AddColumn<int>("New_Id", "TransactionDebits", "int", nullable: true);
            migrationBuilder.AddColumn<int>("New_Id", "TransactionCredits", "int", nullable: true);
            migrationBuilder.AddColumn<int>("New_Id", "PaymentSpecs", "int", nullable: true);
            migrationBuilder.AddColumn<int>("New_Id", "AccountSpecs", "int", nullable: true);

            migrationBuilder.Sql("UPDATE TransactionDebits SET New_Id = Id");
            migrationBuilder.Sql("UPDATE TransactionCredits SET New_Id = Id");
            migrationBuilder.Sql("UPDATE PaymentSpecs SET New_Id = Id");
            migrationBuilder.Sql("UPDATE AccountSpecs SET New_Id = Id");

            migrationBuilder.AlterColumn<int>("New_Id", "TransactionDebits", "int", nullable: false);
            migrationBuilder.AlterColumn<int>("New_Id", "TransactionCredits", "int", nullable: false);
            migrationBuilder.AlterColumn<int>("New_Id", "PaymentSpecs", "int", nullable: false);
            migrationBuilder.AlterColumn<int>("New_Id", "AccountSpecs", "int", nullable: false);

            migrationBuilder.DropColumn("Id", "TransactionDebits");
            migrationBuilder.DropColumn("Id", "TransactionCredits");
            migrationBuilder.DropColumn("Id", "PaymentSpecs");
            migrationBuilder.DropColumn("Id", "AccountSpecs");

            migrationBuilder.RenameColumn("New_Id", "TransactionDebits", "Id");
            migrationBuilder.RenameColumn("New_Id", "TransactionCredits", "Id");
            migrationBuilder.RenameColumn("New_Id", "PaymentSpecs", "Id");
            migrationBuilder.RenameColumn("New_Id", "AccountSpecs", "Id");

            migrationBuilder.AddPrimaryKey(
                "PK_TransactionDebits",
                "TransactionDebits",
                "Id"
            );

            migrationBuilder.AddPrimaryKey(
                "PK_TransactionCredits",
                "TransactionCredits",
                "Id"
            );

            migrationBuilder.AddPrimaryKey(
                "PK_PaymentSpecs",
                "PaymentSpecs",
                "Id"
            );

            migrationBuilder.AddPrimaryKey(
                "PK_AccountSpecs",
                "AccountSpecs",
                "Id"
            );

            migrationBuilder.AddForeignKey(
                "FK_TransactionDebits_AccountSpecs_AccountSpecId",
                "TransactionDebits",
                "AccountSpecId",
                "AccountSpecs",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                "FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId",
                "TransactionDebits",
                "PaymentInstrumentId",
                "PaymentSpecs",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                "FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId",
                "TransactionCredits",
                "PaymentInstrumentId",
                "PaymentSpecs",
                onDelete: ReferentialAction.Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");

            migrationBuilder.DropForeignKey("FK_TransactionDebits_AccountSpecs_AccountSpecId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId", "TransactionCredits");
            migrationBuilder.DropPrimaryKey("PK_TransactionDebits", "TransactionDebits");

            migrationBuilder.DropPrimaryKey("PK_TransactionCredits", "TransactionCredits");
            migrationBuilder.DropPrimaryKey("PK_PaymentSpecs", "PaymentSpecs");
            migrationBuilder.DropPrimaryKey("PK_AccountSpecs", "AccountSpecs");
            migrationBuilder.RenameColumn("Id", "TransactionDebits", "Old_Id");

            migrationBuilder.RenameColumn("Id", "TransactionCredits", "Old_Id");
            migrationBuilder.RenameColumn("Id", "PaymentSpecs", "Old_Id");
            migrationBuilder.RenameColumn("Id", "AccountSpecs", "Old_Id");

            migrationBuilder.AddColumn<int>(
                "Id",
                "TransactionDebits",
                "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                "Id",
                "TransactionCredits",
                "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                "Id",
                "PaymentSpecs",
                "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                "Id",
                "AccountSpecs",
                "int",
                nullable: false
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.Sql("Set IDENTITY_INSERT TransactionDebits ON");
            migrationBuilder.Sql("Set IDENTITY_INSERT TransactionCredits ON");
            migrationBuilder.Sql("Set IDENTITY_INSERT PaymentSpecs ON");
            migrationBuilder.Sql("Set IDENTITY_INSERT AccountSpecs ON");

            // TODO - This is not working, cause IDENTITY column cannot be updated, only inserted this way.
            // Requires explicit coping or ALTER .. SWITCH.

            // migrationBuilder.Sql("UPDATE TransactionDebits SET Id = Old_Id");
            // migrationBuilder.Sql("UPDATE TransactionCredits SET Id = Old_Id");
            // migrationBuilder.Sql("UPDATE PaymentSpecs SET Id = Old_Id");
            // migrationBuilder.Sql("UPDATE AccountSpecs SET Id = Old_Id");

            migrationBuilder.Sql("Set IDENTITY_INSERT TransactionDebits OFF");
            migrationBuilder.Sql("Set IDENTITY_INSERT TransactionCredits OFF");
            migrationBuilder.Sql("Set IDENTITY_INSERT PaymentSpecs OFF");
            migrationBuilder.Sql("Set IDENTITY_INSERT AccountSpecs OFF");

            migrationBuilder.Sql("DBCC CHECKIDENT ('TransactionDebits', RESEED)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('TransactionCredits', RESEED)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('PaymentSpecs', RESEED)");
            migrationBuilder.Sql("DBCC CHECKIDENT ('AccountSpecs', RESEED)");

            migrationBuilder.DropColumn("Old_Id", "TransactionDebits");
            migrationBuilder.DropColumn("Old_Id", "TransactionCredits");
            migrationBuilder.DropColumn("Old_Id", "PaymentSpecs");
            migrationBuilder.DropColumn("Old_Id", "AccountSpecs");

            migrationBuilder.AddPrimaryKey(
                "PK_TransactionDebits",
                "TransactionDebits",
                "Id"
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                "PK_TransactionCredits",
                "TransactionCredits",
                "Id"
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                "PK_PaymentSpecs",
                "PaymentSpecs",
                "Id"
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                "PK_AccountSpecs",
                "AccountSpecs",
                "Id"
            )
            .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                "FK_TransactionDebits_AccountSpecs_AccountSpecId",
                "TransactionDebits",
                "AccountSpecId",
                "AccountSpecs",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                "FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId",
                "TransactionDebits",
                "PaymentInstrumentId",
                "PaymentSpecs",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                "FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId",
                "TransactionCredits",
                "PaymentInstrumentId",
                "PaymentSpecs",
                onDelete: ReferentialAction.Restrict
            );
        }
    }
}
