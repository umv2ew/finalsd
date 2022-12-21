﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using sd.Jatek.Infrastructure;

#nullable disable

namespace sd.Jatek.Infrastructure.Migrations
{
    [DbContext(typeof(GameContext))]
    [Migration("20221219234114_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("sd.Jatek.Domain.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayerRole")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("sd.Jatek.Domain.Room", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RightGuess")
                        .HasColumnType("int");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rounds")
                        .HasColumnType("int");

                    b.Property<bool>("Started")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
