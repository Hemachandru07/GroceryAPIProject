﻿// <auto-generated />
using System;
using GroceryAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GroceryAPI.Migrations
{
    [DbContext(typeof(GroceryContext))]
    [Migration("20220825072845_receipttableupdate")]
    partial class receipttableupdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("GroceryAPI.Models.Admin", b =>
                {
                    b.Property<int>("AdminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminID"), 1L, 1);

                    b.Property<string>("AdminName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminID");

                    b.ToTable("admin");
                });

            modelBuilder.Entity("GroceryAPI.Models.Cart", b =>
                {
                    b.Property<int>("CartID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartID"), 1L, 1);

                    b.Property<string>("CartTypeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("GroceryID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<float?>("UnitPrice")
                        .HasColumnType("real");

                    b.HasKey("CartID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("GroceryID");

                    b.ToTable("carts");
                });

            modelBuilder.Entity("GroceryAPI.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CartTypeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("MobileNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerID");

                    b.ToTable("customer");
                });

            modelBuilder.Entity("GroceryAPI.Models.Grocery", b =>
                {
                    b.Property<int>("GroceryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroceryID"), 1L, 1);

                    b.Property<string>("GroceryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Price")
                        .HasColumnType("real");

                    b.Property<int?>("Stock")
                        .HasColumnType("int");

                    b.HasKey("GroceryID");

                    b.ToTable("grocery");
                });

            modelBuilder.Entity("GroceryAPI.Models.Payment", b =>
                {
                    b.Property<int>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentID"), 1L, 1);

                    b.Property<int>("CardNumber")
                        .HasColumnType("int");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<float?>("TotalAmount")
                        .HasColumnType("real");

                    b.HasKey("PaymentID");

                    b.HasIndex("CustomerID");

                    b.ToTable("payments");
                });

            modelBuilder.Entity("GroceryAPI.Models.Receipt", b =>
                {
                    b.Property<int>("ReceiptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReceiptID"), 1L, 1);

                    b.Property<float?>("Amount")
                        .HasColumnType("real");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("GroceryID")
                        .HasColumnType("int");

                    b.Property<int?>("PaymentID")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReceiptDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ReceiptID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("GroceryID");

                    b.HasIndex("PaymentID");

                    b.ToTable("receipts");
                });

            modelBuilder.Entity("GroceryAPI.Models.Cart", b =>
                {
                    b.HasOne("GroceryAPI.Models.Customer", "customer")
                        .WithMany("cart")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GroceryAPI.Models.Grocery", "grocery")
                        .WithMany("cart")
                        .HasForeignKey("GroceryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");

                    b.Navigation("grocery");
                });

            modelBuilder.Entity("GroceryAPI.Models.Payment", b =>
                {
                    b.HasOne("GroceryAPI.Models.Customer", "customer")
                        .WithMany("Payment")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");
                });

            modelBuilder.Entity("GroceryAPI.Models.Receipt", b =>
                {
                    b.HasOne("GroceryAPI.Models.Customer", "customer")
                        .WithMany("Receipt")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GroceryAPI.Models.Grocery", "grocery")
                        .WithMany("receipt")
                        .HasForeignKey("GroceryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GroceryAPI.Models.Payment", null)
                        .WithMany("Receipt")
                        .HasForeignKey("PaymentID");

                    b.Navigation("customer");

                    b.Navigation("grocery");
                });

            modelBuilder.Entity("GroceryAPI.Models.Customer", b =>
                {
                    b.Navigation("Payment");

                    b.Navigation("Receipt");

                    b.Navigation("cart");
                });

            modelBuilder.Entity("GroceryAPI.Models.Grocery", b =>
                {
                    b.Navigation("cart");

                    b.Navigation("receipt");
                });

            modelBuilder.Entity("GroceryAPI.Models.Payment", b =>
                {
                    b.Navigation("Receipt");
                });
#pragma warning restore 612, 618
        }
    }
}
