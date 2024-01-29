﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Services.PlantsAPI.DbContexts;

#nullable disable

namespace Services.PlantsAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230802102504_ImageUrlsMovedToTable")]
    partial class ImageUrlsMovedToTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Services.PlantsAPI.Models.ImageLink", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ImageId"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PlantId")
                        .HasColumnType("integer");

                    b.Property<string>("ViewType")
                        .IsRequired()
                        .HasColumnType("VARCHAR");

                    b.HasKey("ImageId");

                    b.HasIndex("PlantId");

                    b.ToTable("ImageLink");
                });

            modelBuilder.Entity("Services.PlantsAPI.Models.Plant", b =>
                {
                    b.Property<int>("PlantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlantId"));

                    b.Property<int>("FlowerColorCode")
                        .HasColumnType("integer");

                    b.Property<bool>("ForHerbalTea")
                        .HasColumnType("boolean");

                    b.Property<bool>("PickingProhibited")
                        .HasColumnType("boolean");

                    b.Property<bool>("Poisonous")
                        .HasColumnType("boolean");

                    b.HasKey("PlantId");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("Services.PlantsAPI.Models.PlantName", b =>
                {
                    b.Property<int>("PlantNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlantNameId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PlantId")
                        .HasColumnType("integer");

                    b.HasKey("PlantNameId");

                    b.HasIndex("PlantId");

                    b.ToTable("PlantNames");
                });

            modelBuilder.Entity("Services.PlantsAPI.Models.ImageLink", b =>
                {
                    b.HasOne("Services.PlantsAPI.Models.Plant", "Plant")
                        .WithMany("ImageLinks")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("Services.PlantsAPI.Models.PlantName", b =>
                {
                    b.HasOne("Services.PlantsAPI.Models.Plant", "Plant")
                        .WithMany("Names")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("Services.PlantsAPI.Models.Plant", b =>
                {
                    b.Navigation("ImageLinks");

                    b.Navigation("Names");
                });
#pragma warning restore 612, 618
        }
    }
}
