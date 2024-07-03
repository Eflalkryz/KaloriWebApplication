﻿// <auto-generated />
using System;
using KaloriWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KaloriWebApplication.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.Calory", b =>
                {
                    b.Property<int>("CaloryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CaloryID"));

                    b.Property<DateTime>("CaloryEntryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int?>("CustomersProfilesCustomerID")
                        .HasColumnType("int");

                    b.Property<int?>("NutrientsNutrientID")
                        .HasColumnType("int");

                    b.Property<int>("TotalCalory")
                        .HasColumnType("int");

                    b.HasKey("CaloryID");

                    b.HasIndex("CustomersProfilesCustomerID");

                    b.HasIndex("NutrientsNutrientID");

                    b.ToTable("Calories");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.CustomersProfile", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<string>("ActivityLevel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("DailyCaloryLimit")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Goal")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("CustomerID");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("CustomersProfiles");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.Nutrient", b =>
                {
                    b.Property<int>("NutrientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NutrientID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int?>("CustomersProfileCustomerID")
                        .HasColumnType("int");

                    b.Property<int>("NutrientAmount")
                        .HasColumnType("int");

                    b.Property<int>("NutrientCalory")
                        .HasColumnType("int");

                    b.Property<DateTime>("NutrientEntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NutrientName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NutrientID");

                    b.HasIndex("CustomersProfileCustomerID");

                    b.ToTable("Nutrients");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<bool>("AdminRole")
                        .HasColumnType("bit");

                    b.Property<string>("Eposta")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.Calory", b =>
                {
                    b.HasOne("KaloriWebApplication.Models.Concrete.CustomersProfile", "CustomersProfiles")
                        .WithMany("Calories")
                        .HasForeignKey("CustomersProfilesCustomerID");

                    b.HasOne("KaloriWebApplication.Models.Concrete.Nutrient", "Nutrients")
                        .WithMany()
                        .HasForeignKey("NutrientsNutrientID");

                    b.Navigation("CustomersProfiles");

                    b.Navigation("Nutrients");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.CustomersProfile", b =>
                {
                    b.HasOne("KaloriWebApplication.Models.Concrete.User", "Users")
                        .WithOne("CustomersProfiles")
                        .HasForeignKey("KaloriWebApplication.Models.Concrete.CustomersProfile", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.Nutrient", b =>
                {
                    b.HasOne("KaloriWebApplication.Models.Concrete.CustomersProfile", "CustomersProfile")
                        .WithMany("Nutrients")
                        .HasForeignKey("CustomersProfileCustomerID");

                    b.Navigation("CustomersProfile");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.CustomersProfile", b =>
                {
                    b.Navigation("Calories");

                    b.Navigation("Nutrients");
                });

            modelBuilder.Entity("KaloriWebApplication.Models.Concrete.User", b =>
                {
                    b.Navigation("CustomersProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}
