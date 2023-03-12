﻿// <auto-generated />
using System;
using ChairsBackend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChairsBackend.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("ChairsBackend.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameId1")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("GameId1");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("ChairsBackend.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Bank")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ChairsBackend.Entities.Player", b =>
                {
                    b.HasOne("ChairsBackend.Models.Game", null)
                        .WithMany("Players")
                        .HasForeignKey("GameId");

                    b.HasOne("ChairsBackend.Models.Game", null)
                        .WithMany("Winners")
                        .HasForeignKey("GameId1");
                });

            modelBuilder.Entity("ChairsBackend.Models.Game", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Winners");
                });
#pragma warning restore 612, 618
        }
    }
}
