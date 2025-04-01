﻿// <auto-generated />
using Api.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    partial class ProductsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Api.Models.Auth.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "$2a$11$M5oW82FBNXNkmXNEoEo7hepd.Nnz2vkFEz/riZUKSgH8T6MgrAxda",
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Password = "$2a$11$.sMJ./zBiI4quAi6F4mqwurhB8H3IlTYBcDVCo/O1nREh7dBkF1M.",
                            Username = "viewer"
                        });
                });

            modelBuilder.Entity("Api.Models.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Description 1",
                            Name = "Product 1",
                            Price = 10.00m,
                            Stock = 100
                        },
                        new
                        {
                            Id = 2,
                            Description = "Description 2",
                            Name = "Product 2",
                            Price = 20.00m,
                            Stock = 200
                        },
                        new
                        {
                            Id = 3,
                            Description = "Description 3",
                            Name = "Product 3",
                            Price = 30.00m,
                            Stock = 300
                        },
                        new
                        {
                            Id = 4,
                            Description = "Description 4",
                            Name = "Product 4",
                            Price = 40.00m,
                            Stock = 400
                        },
                        new
                        {
                            Id = 5,
                            Description = "Description 5",
                            Name = "Product 5",
                            Price = 50.00m,
                            Stock = 500
                        },
                        new
                        {
                            Id = 6,
                            Description = "Description 6",
                            Name = "Product 6",
                            Price = 60.00m,
                            Stock = 600
                        },
                        new
                        {
                            Id = 7,
                            Description = "Description 7",
                            Name = "Product 7",
                            Price = 70.00m,
                            Stock = 700
                        },
                        new
                        {
                            Id = 8,
                            Description = "Description 8",
                            Name = "Product 8",
                            Price = 80.00m,
                            Stock = 800
                        },
                        new
                        {
                            Id = 9,
                            Description = "Description 9",
                            Name = "Product 9",
                            Price = 90.00m,
                            Stock = 900
                        },
                        new
                        {
                            Id = 10,
                            Description = "Description 10",
                            Name = "Product 10",
                            Price = 100.00m,
                            Stock = 1000
                        },
                        new
                        {
                            Id = 11,
                            Description = "Description 11",
                            Name = "Product 11",
                            Price = 110.00m,
                            Stock = 1100
                        },
                        new
                        {
                            Id = 12,
                            Description = "Description 12",
                            Name = "Product 12",
                            Price = 120.00m,
                            Stock = 1200
                        },
                        new
                        {
                            Id = 13,
                            Description = "Description 13",
                            Name = "Product 13",
                            Price = 130.00m,
                            Stock = 1300
                        },
                        new
                        {
                            Id = 14,
                            Description = "Description 14",
                            Name = "Product 14",
                            Price = 140.00m,
                            Stock = 1400
                        },
                        new
                        {
                            Id = 15,
                            Description = "Description 15",
                            Name = "Product 15",
                            Price = 150.00m,
                            Stock = 1500
                        },
                        new
                        {
                            Id = 16,
                            Description = "Description 16",
                            Name = "Product 16",
                            Price = 160.00m,
                            Stock = 1600
                        },
                        new
                        {
                            Id = 17,
                            Description = "Description 17",
                            Name = "Product 17",
                            Price = 170.00m,
                            Stock = 1700
                        },
                        new
                        {
                            Id = 18,
                            Description = "Description 18",
                            Name = "Product 18",
                            Price = 180.00m,
                            Stock = 1800
                        },
                        new
                        {
                            Id = 19,
                            Description = "Description 19",
                            Name = "Product 19",
                            Price = 190.00m,
                            Stock = 1900
                        },
                        new
                        {
                            Id = 20,
                            Description = "Description 20",
                            Name = "Product 20",
                            Price = 200.00m,
                            Stock = 2000
                        },
                        new
                        {
                            Id = 21,
                            Description = "Description 21",
                            Name = "Product 21",
                            Price = 210.00m,
                            Stock = 2100
                        },
                        new
                        {
                            Id = 22,
                            Description = "Description 22",
                            Name = "Product 22",
                            Price = 220.00m,
                            Stock = 2200
                        },
                        new
                        {
                            Id = 23,
                            Description = "Description 23",
                            Name = "Product 23",
                            Price = 230.00m,
                            Stock = 2300
                        },
                        new
                        {
                            Id = 24,
                            Description = "Description 24",
                            Name = "Product 24",
                            Price = 240.00m,
                            Stock = 2400
                        },
                        new
                        {
                            Id = 25,
                            Description = "Description 25",
                            Name = "Product 25",
                            Price = 250.00m,
                            Stock = 2500
                        },
                        new
                        {
                            Id = 26,
                            Description = "Description 26",
                            Name = "Product 26",
                            Price = 260.00m,
                            Stock = 2600
                        },
                        new
                        {
                            Id = 27,
                            Description = "Description 27",
                            Name = "Product 27",
                            Price = 270.00m,
                            Stock = 2700
                        },
                        new
                        {
                            Id = 28,
                            Description = "Description 28",
                            Name = "Product 28",
                            Price = 280.00m,
                            Stock = 2800
                        },
                        new
                        {
                            Id = 29,
                            Description = "Description 29",
                            Name = "Product 29",
                            Price = 290.00m,
                            Stock = 2900
                        },
                        new
                        {
                            Id = 30,
                            Description = "Description 30",
                            Name = "Product 30",
                            Price = 300.00m,
                            Stock = 3000
                        },
                        new
                        {
                            Id = 31,
                            Description = "Description 31",
                            Name = "Product 31",
                            Price = 310.00m,
                            Stock = 3100
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
