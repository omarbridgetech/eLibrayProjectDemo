﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eLibrayProjectDemo.Services;

#nullable disable

namespace eLibrayProjectDemo.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241207175223_ChangeAgeRatingToString")]
    partial class ChangeAgeRatingToString
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("eLibrayProjectDemo.Models.Book", b =>
                {
                    b.Property<int>("EbookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EbookId"));

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("AgeRating")
                        .HasColumnType("int");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("AvailableCopies")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverImagePath")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DiscountStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EpubFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("F2bFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Formats")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBuyOnly")
                        .HasColumnType("bit");

                    b.Property<string>("MobiFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PdfFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("PreviousPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceBorrow")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PriceBuy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PublicationYear")
                        .HasColumnType("int");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("EbookId");

                    b.ToTable("Books");
                });
#pragma warning restore 612, 618
        }
    }
}