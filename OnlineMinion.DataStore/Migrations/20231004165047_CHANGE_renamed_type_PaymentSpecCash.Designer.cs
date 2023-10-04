﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineMinion.DataStore;

#nullable disable

namespace OnlineMinion.DataStore.Migrations
{
    [DbContext(typeof(OnlineMinionDbContext))]
    [Migration("20231004165047_CHANGE_renamed_type_PaymentSpecCash")]
    partial class CHANGE_renamed_type_PaymentSpecCash
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.1.23419.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineMinion.Domain.AccountSpecs.AccountSpec", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

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

            modelBuilder.Entity("OnlineMinion.Domain.PaymentSpecs.BasePaymentSpecData", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tags")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PaymentSpecs", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("BasePaymentSpecData");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("OnlineMinion.Domain.TransactionCredits.TransactionCredit", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Party")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<Guid>("PaymentInstrumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tags")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("PaymentInstrumentId");

                    b.ToTable("TransactionCredits");

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("OnlineMinion.Domain.TransactionDebits.TransactionDebit", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountSpecId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("Fee")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Party")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<Guid>("PaymentInstrumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Tags")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("AccountSpecId");

                    b.HasIndex("PaymentInstrumentId");

                    b.ToTable("TransactionDebits");

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("OnlineMinion.Domain.PaymentSpecs.BankAccountSpec", b =>
                {
                    b.HasBaseType("OnlineMinion.Domain.PaymentSpecs.BasePaymentSpecData");

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

            modelBuilder.Entity("OnlineMinion.Domain.PaymentSpecs.CryptoExchangeAccountSpec", b =>
                {
                    b.HasBaseType("OnlineMinion.Domain.PaymentSpecs.BasePaymentSpecData");

                    b.Property<string>("ExchangeName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<bool>("IsFiat")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("CryptoExchangeAccountSpec");
                });

            modelBuilder.Entity("OnlineMinion.Domain.PaymentSpecs.PaymentSpecCash", b =>
                {
                    b.HasBaseType("OnlineMinion.Domain.PaymentSpecs.BasePaymentSpecData");

                    b.HasDiscriminator().HasValue("PaymentSpecCash");
                });

            modelBuilder.Entity("OnlineMinion.Domain.TransactionCredits.TransactionCredit", b =>
                {
                    b.HasOne("OnlineMinion.Domain.PaymentSpecs.BasePaymentSpecData", "PaymentInstrument")
                        .WithMany()
                        .HasForeignKey("PaymentInstrumentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("PaymentInstrument");
                });

            modelBuilder.Entity("OnlineMinion.Domain.TransactionDebits.TransactionDebit", b =>
                {
                    b.HasOne("OnlineMinion.Domain.AccountSpecs.AccountSpec", "AccountSpec")
                        .WithMany()
                        .HasForeignKey("AccountSpecId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OnlineMinion.Domain.PaymentSpecs.BasePaymentSpecData", "PaymentInstrument")
                        .WithMany()
                        .HasForeignKey("PaymentInstrumentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountSpec");

                    b.Navigation("PaymentInstrument");
                });
#pragma warning restore 612, 618
        }
    }
}
