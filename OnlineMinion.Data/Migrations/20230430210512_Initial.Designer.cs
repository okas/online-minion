﻿// <auto-generated />
using System;
using OnlineMinion.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace OnlineMinion.Data.Migrations
{
    [DbContext(typeof(OnlineMinionDbContext))]
    [Migration("20230430210512_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.3.23174.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineMinion.Data.BaseEntities.BasePaymentSpec", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CurrencyIso")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentSpecs");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BasePaymentSpec");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("OnlineMinion.Data.BaseEntities.BaseTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Party")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<int>("PaymentInstrumentId")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PaymentInstrumentId")
                        .IsUnique();

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("OnlineMinion.Data.Entities.AccountSpec", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AccountSpecs");
                });

            modelBuilder.Entity("OnlineMinion.Data.Entities.BankAccountSpec", b =>
                {
                    b.HasBaseType("OnlineMinion.Data.BaseEntities.BasePaymentSpec");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("IBAN")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.HasDiscriminator().HasValue("BankAccountSpec");
                });

            modelBuilder.Entity("OnlineMinion.Data.Entities.CashAccountSpec", b =>
                {
                    b.HasBaseType("OnlineMinion.Data.BaseEntities.BasePaymentSpec");

                    b.HasDiscriminator().HasValue("CashAccountSpec");
                });

            modelBuilder.Entity("OnlineMinion.Data.Entities.TransactionCredit", b =>
                {
                    b.HasBaseType("OnlineMinion.Data.BaseEntities.BaseTransaction");

                    b.ToTable("TransactionCredits");
                });

            modelBuilder.Entity("OnlineMinion.Data.Entities.TransactionDebit", b =>
                {
                    b.HasBaseType("OnlineMinion.Data.BaseEntities.BaseTransaction");

                    b.Property<int>("AccountSpecId")
                        .HasColumnType("int");

                    b.Property<decimal>("Fee")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasIndex("AccountSpecId")
                        .IsUnique()
                        .HasFilter("[AccountSpecId] IS NOT NULL");

                    b.ToTable("TransactionDebits");
                });

            modelBuilder.Entity("OnlineMinion.Data.BaseEntities.BaseTransaction", b =>
                {
                    b.HasOne("OnlineMinion.Data.BaseEntities.BasePaymentSpec", "PaymentInstrument")
                        .WithOne()
                        .HasForeignKey("OnlineMinion.Data.BaseEntities.BaseTransaction", "PaymentInstrumentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("PaymentInstrument");
                });

            modelBuilder.Entity("OnlineMinion.Data.Entities.TransactionDebit", b =>
                {
                    b.HasOne("OnlineMinion.Data.Entities.AccountSpec", "AccountSpec")
                        .WithOne()
                        .HasForeignKey("OnlineMinion.Data.Entities.TransactionDebit", "AccountSpecId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountSpec");
                });
#pragma warning restore 612, 618
        }
    }
}
