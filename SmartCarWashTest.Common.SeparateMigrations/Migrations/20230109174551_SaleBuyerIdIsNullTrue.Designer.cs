﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartCarWashTest.Common.DataContext.Sqlite;

namespace SmartCarWashTest.Common.SeparateMigrations.Migrations
{
    [DbContext(typeof(SmartCarWashContext))]
    [Migration("20230109174551_SaleBuyerIdIsNullTrue")]
    partial class SaleBuyerIdIsNullTrue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Buyer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Buyer");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.ProvidedProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SalesPointId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalesPointId");

                    b.ToTable("ProvidedProduct");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BuyerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("SalesPointId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("TEXT");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("SalesPointId");

                    b.ToTable("Sale");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.SalesData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductIdAmount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SaleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SaleId");

                    b.ToTable("SalesData");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.SalesPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SalesPoint");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.ProvidedProduct", b =>
                {
                    b.HasOne("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Product", "Product")
                        .WithMany("ProvidedProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.SalesPoint", "SalePoint")
                        .WithMany("ProvidedProduct")
                        .HasForeignKey("SalesPointId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("SalePoint");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Sale", b =>
                {
                    b.HasOne("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Buyer", "Buyer")
                        .WithMany("Sales")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.SalesPoint", "SalesPoint")
                        .WithMany("Sales")
                        .HasForeignKey("SalesPointId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("SalesPoint");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.SalesData", b =>
                {
                    b.HasOne("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Product", "Product")
                        .WithMany("SalesDataSet")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Sale", "Sale")
                        .WithMany("SalesDataSet")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Buyer", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Product", b =>
                {
                    b.Navigation("ProvidedProducts");

                    b.Navigation("SalesDataSet");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.Sale", b =>
                {
                    b.Navigation("SalesDataSet");
                });

            modelBuilder.Entity("SmartCarWashTest.Common.EntityModels.Sqlite.Entities.SalesPoint", b =>
                {
                    b.Navigation("ProvidedProduct");

                    b.Navigation("Sales");
                });
#pragma warning restore 612, 618
        }
    }
}
