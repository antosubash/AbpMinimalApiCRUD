﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.EntityFrameworkCore;

#nullable disable

namespace MinimalAbpCRUD.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.Sqlite)
                .HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("Book", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT")
                        .HasColumnName("ConcurrencyStamp");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("TEXT")
                        .HasColumnName("ExtraProperties");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Books", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("ac6fddef-6ef0-412f-b58d-dec6007e8df7"),
                            ConcurrencyStamp = "cff00749d0364a48b4a766da77493607",
                            ExtraProperties = "{}",
                            Name = "My Book"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
