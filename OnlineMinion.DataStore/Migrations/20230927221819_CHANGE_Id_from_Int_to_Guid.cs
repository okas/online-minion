using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class CHANGE_Id_from_Int_to_Guid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");

           // Drop Indices
           migrationBuilder.DropIndex("IX_AccountSpecs_Name", "AccountSpecs");
           migrationBuilder.DropIndex("IX_PaymentSpecs_Name", "PaymentSpecs");
           migrationBuilder.DropIndex("IX_TransactionCredits_PaymentInstrumentId", "TransactionCredits");
           migrationBuilder.DropIndex("IX_TransactionDebits_AccountSpecId", "TransactionDebits");
           migrationBuilder.DropIndex("IX_TransactionDebits_PaymentInstrumentId", "TransactionDebits");

           // Drop FKs
            migrationBuilder.DropForeignKey("FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionDebits_AccountSpecs_AccountSpecId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId", "TransactionCredits");

            // Drop PKs
            migrationBuilder.DropPrimaryKey("PK_TransactionDebits", "TransactionDebits");
            migrationBuilder.DropPrimaryKey("PK_TransactionCredits", "TransactionCredits");
            migrationBuilder.DropPrimaryKey("PK_PaymentSpecs", "PaymentSpecs");
            migrationBuilder.DropPrimaryKey("PK_AccountSpecs", "AccountSpecs");

            // Rename the old PKs columns to "Old_" + name
            migrationBuilder.RenameColumn("Id", "TransactionDebits", "Old_Id");
            migrationBuilder.RenameColumn("Id", "TransactionCredits", "Old_Id");
            migrationBuilder.RenameColumn("Id", "PaymentSpecs", "Old_Id");
            migrationBuilder.RenameColumn("Id", "AccountSpecs", "Old_Id");

            // Add new PKs columns with the same name as the old ones, allow nulls
            migrationBuilder.AddColumn<Guid>("Id", "TransactionDebits", "uniqueidentifier", nullable: true);
            migrationBuilder.AddColumn<Guid>("Id", "TransactionCredits", "uniqueidentifier", nullable: true);
            migrationBuilder.AddColumn<Guid>("Id", "PaymentSpecs", "uniqueidentifier", nullable: true);
            migrationBuilder.AddColumn<Guid>("Id", "AccountSpecs", "uniqueidentifier", nullable: true);

            // Generate new IDs
            migrationBuilder.Sql("UPDATE TransactionDebits SET Id = NEWID()");
            migrationBuilder.Sql("UPDATE TransactionCredits SET Id = NEWID()");
            migrationBuilder.Sql("UPDATE PaymentSpecs SET Id = NEWID()");
            migrationBuilder.Sql("UPDATE AccountSpecs SET Id = NEWID()");

            // Modify the new PKs columns to not allow nulls
            migrationBuilder.AlterColumn<Guid>("Id", "TransactionDebits", "uniqueidentifier", nullable: false);
            migrationBuilder.AlterColumn<Guid>("Id", "TransactionCredits", "uniqueidentifier", nullable: false);
            migrationBuilder.AlterColumn<Guid>("Id", "PaymentSpecs", "uniqueidentifier", nullable: false);
            migrationBuilder.AlterColumn<Guid>("Id", "AccountSpecs", "uniqueidentifier", nullable: false);

            // Rename the old FKs columns to "Old_" + name
            migrationBuilder.RenameColumn("AccountSpecId", "TransactionDebits", "Old_AccountSpecId");
            migrationBuilder.RenameColumn("PaymentInstrumentId", "TransactionDebits", "Old_PaymentInstrumentId");
            migrationBuilder.RenameColumn("PaymentInstrumentId", "TransactionCredits", "Old_PaymentInstrumentId");

            // Create new FKs columns with the same name as the old ones, allow nulls
            migrationBuilder.AddColumn<Guid>("AccountSpecId", "TransactionDebits", "uniqueidentifier", nullable: true);
            migrationBuilder.AddColumn<Guid>("PaymentInstrumentId", "TransactionDebits", "uniqueidentifier", nullable: true);
            migrationBuilder.AddColumn<Guid>("PaymentInstrumentId", "TransactionCredits", "uniqueidentifier", nullable: true);

            // Relate the new FKs columns: use old FKs values to find the new FKs values
            migrationBuilder.Sql("UPDATE TransactionDebits SET AccountSpecId = (SELECT Id FROM AccountSpecs WHERE Old_Id = TransactionDebits.Old_AccountSpecId)");
            migrationBuilder.Sql("UPDATE TransactionDebits SET PaymentInstrumentId = (SELECT Id FROM PaymentSpecs WHERE Old_Id = TransactionDebits.Old_PaymentInstrumentId)");
            migrationBuilder.Sql("UPDATE TransactionCredits SET PaymentInstrumentId = (SELECT Id FROM PaymentSpecs WHERE Old_Id = TransactionCredits.Old_PaymentInstrumentId)");

            // Modify the new FKs columns to not allow nulls
            migrationBuilder.AlterColumn<Guid>("AccountSpecId", "TransactionDebits", "uniqueidentifier", nullable: false);
            migrationBuilder.AlterColumn<Guid>("PaymentInstrumentId", "TransactionDebits", "uniqueidentifier", nullable: false);
            migrationBuilder.AlterColumn<Guid>("PaymentInstrumentId", "TransactionCredits", "uniqueidentifier", nullable: false);

            // Create the new PKs
            migrationBuilder.AddPrimaryKey("PK_TransactionDebits", "TransactionDebits", "Id");
            migrationBuilder.AddPrimaryKey("PK_TransactionCredits", "TransactionCredits", "Id");
            migrationBuilder.AddPrimaryKey("PK_PaymentSpecs", "PaymentSpecs", "Id");
            migrationBuilder.AddPrimaryKey("PK_AccountSpecs", "AccountSpecs", "Id");

            // Drop the old FKs columns
            migrationBuilder.DropColumn("Old_AccountSpecId", "TransactionDebits");
            migrationBuilder.DropColumn("Old_PaymentInstrumentId", "TransactionDebits");
            migrationBuilder.DropColumn("Old_PaymentInstrumentId", "TransactionCredits");

            // Add the new FKs
            migrationBuilder.AddForeignKey("FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId", "TransactionDebits", "PaymentInstrumentId", "PaymentSpecs", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey("FK_TransactionDebits_AccountSpecs_AccountSpecId", "TransactionDebits", "AccountSpecId", "AccountSpecs", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey("FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId", "TransactionCredits", "PaymentInstrumentId", "PaymentSpecs", principalColumn: "Id", onDelete: ReferentialAction.Restrict);

            // Drop the old ID columns
            migrationBuilder.DropColumn("Old_Id", "TransactionDebits");
            migrationBuilder.DropColumn("Old_Id", "TransactionCredits");
            migrationBuilder.DropColumn("Old_Id", "PaymentSpecs");
            migrationBuilder.DropColumn("Old_Id", "AccountSpecs");

            // Create indices
            migrationBuilder.CreateIndex("IX_AccountSpecs_Name", "AccountSpecs", "Name", unique: true);
            migrationBuilder.CreateIndex("IX_PaymentSpecs_Name", "PaymentSpecs", "Name", unique: true);
            migrationBuilder.CreateIndex("IX_TransactionCredits_PaymentInstrumentId", "TransactionCredits", "PaymentInstrumentId", unique: false);
            migrationBuilder.CreateIndex("IX_TransactionDebits_AccountSpecId", "TransactionDebits", "AccountSpecId", unique: false);
            migrationBuilder.CreateIndex("IX_TransactionDebits_PaymentInstrumentId", "TransactionDebits", "PaymentInstrumentId", unique: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Create new sequence with start value of 1 and increment of 10
            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                startValue: 1L,
                incrementBy: 10);

            // Drop the new indices
            migrationBuilder.DropIndex("IX_AccountSpecs_Name", "AccountSpecs");
            migrationBuilder.DropIndex("IX_PaymentSpecs_Name", "PaymentSpecs");
            migrationBuilder.DropIndex("IX_TransactionCredits_PaymentInstrumentId", "TransactionCredits");
            migrationBuilder.DropIndex("IX_TransactionDebits_AccountSpecId", "TransactionDebits");
            migrationBuilder.DropIndex("IX_TransactionDebits_PaymentInstrumentId", "TransactionDebits");

            // Drop the new FKs
            migrationBuilder.DropForeignKey("FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionDebits_AccountSpecs_AccountSpecId", "TransactionDebits");
            migrationBuilder.DropForeignKey("FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId", "TransactionCredits");

            // Rename the old FKs columns to "Old_" + name
            migrationBuilder.RenameColumn("AccountSpecId", "TransactionDebits", "Old_AccountSpecId");
            migrationBuilder.RenameColumn("PaymentInstrumentId", "TransactionDebits", "Old_PaymentInstrumentId");
            migrationBuilder.RenameColumn("PaymentInstrumentId", "TransactionCredits", "Old_PaymentInstrumentId");

            // Create new FKs columns with the same name as the old ones, allow nulls
            migrationBuilder.AddColumn<int>("AccountSpecId", "TransactionDebits", "int", nullable: true);
            migrationBuilder.AddColumn<int>("PaymentInstrumentId", "TransactionDebits", "int", nullable: true);
            migrationBuilder.AddColumn<int>("PaymentInstrumentId", "TransactionCredits", "int", nullable: true);

            // Drop PKs
            migrationBuilder.DropPrimaryKey("PK_TransactionDebits", "TransactionDebits");
            migrationBuilder.DropPrimaryKey("PK_TransactionCredits", "TransactionCredits");
            migrationBuilder.DropPrimaryKey("PK_PaymentSpecs", "PaymentSpecs");
            migrationBuilder.DropPrimaryKey("PK_AccountSpecs", "AccountSpecs");

            // Rename the old PKs columns to "Old_" + name
            migrationBuilder.RenameColumn("Id", "TransactionDebits", "Old_Id");
            migrationBuilder.RenameColumn("Id", "TransactionCredits", "Old_Id");
            migrationBuilder.RenameColumn("Id", "PaymentSpecs", "Old_Id");
            migrationBuilder.RenameColumn("Id", "AccountSpecs", "Old_Id");

            // Add new PKs columns with the same name as the old ones, allow nulls
            migrationBuilder.AddColumn<int>("Id", "TransactionDebits", "int", nullable: true);
            migrationBuilder.AddColumn<int>("Id", "TransactionCredits", "int", nullable: true);
            migrationBuilder.AddColumn<int>("Id", "PaymentSpecs", "int", nullable: true);
            migrationBuilder.AddColumn<int>("Id", "AccountSpecs", "int", nullable: true);

            // Generate new IDs
            migrationBuilder.Sql("UPDATE TransactionDebits SET Id = NEXT VALUE FOR EntityFrameworkHiLoSequence");
            migrationBuilder.Sql("UPDATE TransactionCredits SET Id = NEXT VALUE FOR EntityFrameworkHiLoSequence");
            migrationBuilder.Sql("UPDATE PaymentSpecs SET Id = NEXT VALUE FOR EntityFrameworkHiLoSequence");
            migrationBuilder.Sql("UPDATE AccountSpecs SET Id = NEXT VALUE FOR EntityFrameworkHiLoSequence");

            // Modify the new PKs columns to not allow nulls
            migrationBuilder.AlterColumn<int>("Id", "TransactionDebits", "int", nullable: false);
            migrationBuilder.AlterColumn<int>("Id", "TransactionCredits", "int", nullable: false);
            migrationBuilder.AlterColumn<int>("Id", "PaymentSpecs", "int", nullable: false);
            migrationBuilder.AlterColumn<int>("Id", "AccountSpecs", "int", nullable: false);

            // Restore relations: use old FKs values to find the new FKs values
            migrationBuilder.Sql("UPDATE TransactionDebits SET AccountSpecId = (SELECT Id FROM AccountSpecs WHERE Old_Id = TransactionDebits.Old_AccountSpecId)");
            migrationBuilder.Sql("UPDATE TransactionDebits SET PaymentInstrumentId = (SELECT Id FROM PaymentSpecs WHERE Old_Id = TransactionDebits.Old_PaymentInstrumentId)");
            migrationBuilder.Sql("UPDATE TransactionCredits SET PaymentInstrumentId = (SELECT Id FROM PaymentSpecs WHERE Old_Id = TransactionCredits.Old_PaymentInstrumentId)");

            // Add new PKs
            migrationBuilder.AddPrimaryKey("PK_TransactionDebits", "TransactionDebits", "Id");
            migrationBuilder.AddPrimaryKey("PK_TransactionCredits", "TransactionCredits", "Id");
            migrationBuilder.AddPrimaryKey("PK_PaymentSpecs", "PaymentSpecs", "Id");
            migrationBuilder.AddPrimaryKey("PK_AccountSpecs", "AccountSpecs", "Id");

            // Add new FKs
            migrationBuilder.AddForeignKey("FK_TransactionDebits_PaymentSpecs_PaymentInstrumentId", "TransactionDebits", "PaymentInstrumentId", "PaymentSpecs", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey("FK_TransactionDebits_AccountSpecs_AccountSpecId", "TransactionDebits", "AccountSpecId", "AccountSpecs", principalColumn: "Id", onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey("FK_TransactionCredits_PaymentSpecs_PaymentInstrumentId", "TransactionCredits", "PaymentInstrumentId", "PaymentSpecs", principalColumn: "Id", onDelete: ReferentialAction.Restrict);

            // Drop the old FKs columns
            migrationBuilder.DropColumn("Old_AccountSpecId", "TransactionDebits");
            migrationBuilder.DropColumn("Old_PaymentInstrumentId", "TransactionDebits");
            migrationBuilder.DropColumn("Old_PaymentInstrumentId", "TransactionCredits");

            // Drop the old ID columns
            migrationBuilder.DropColumn("Old_Id", "TransactionDebits");
            migrationBuilder.DropColumn("Old_Id", "TransactionCredits");
            migrationBuilder.DropColumn("Old_Id", "PaymentSpecs");
            migrationBuilder.DropColumn("Old_Id", "AccountSpecs");

            // Create indices
            migrationBuilder.CreateIndex("IX_AccountSpecs_Name", "AccountSpecs", "Name", unique: true);
            migrationBuilder.CreateIndex("IX_PaymentSpecs_Name", "PaymentSpecs", "Name", unique: true);
            migrationBuilder.CreateIndex("IX_TransactionCredits_PaymentInstrumentId", "TransactionCredits", "PaymentInstrumentId", unique: false);
            migrationBuilder.CreateIndex("IX_TransactionDebits_AccountSpecId", "TransactionDebits", "AccountSpecId", unique: false);
            migrationBuilder.CreateIndex("IX_TransactionDebits_PaymentInstrumentId", "TransactionDebits", "PaymentInstrumentId", unique: false);
        }
    }
}
