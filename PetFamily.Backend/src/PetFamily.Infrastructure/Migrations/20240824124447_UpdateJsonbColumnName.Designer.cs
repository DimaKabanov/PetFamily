﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.Infrastructure;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240824124447_UpdateJsonbColumnName")]
    partial class UpdateJsonbColumnName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.Domain.Models.Pets.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("address");

                    b.Property<int>("AssistanceStatus")
                        .HasColumnType("integer")
                        .HasColumnName("assistance_status");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("breed");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("color");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("description");

                    b.Property<string>("Health")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("health");

                    b.Property<double>("Height")
                        .HasColumnType("double precision")
                        .HasColumnName("height");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("phone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("type");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.Models.Volunteers.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("description");

                    b.Property<int>("Experience")
                        .HasColumnType("integer")
                        .HasColumnName("experience");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("full_name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("phone");

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.Models.Pets.Pet", b =>
                {
                    b.HasOne("PetFamily.Domain.Models.Volunteers.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("PetFamily.Domain.Models.Pets.Detail", "Details", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("details");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("PetFamily.Domain.Models.Requisite", "Requisites", b2 =>
                                {
                                    b2.Property<Guid>("DetailPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(2000)
                                        .HasColumnType("character varying(2000)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)");

                                    b2.HasKey("DetailPetId", "Id")
                                        .HasName("pk_pets");

                                    b2.ToTable("pets");

                                    b2.WithOwner()
                                        .HasForeignKey("DetailPetId")
                                        .HasConstraintName("fk_pets_pets_detail_pet_id");
                                });

                            b1.OwnsMany("PetFamily.Domain.Models.Pets.PetPhoto", "Photos", b2 =>
                                {
                                    b2.Property<Guid>("DetailPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<bool>("IsMain")
                                        .HasColumnType("boolean");

                                    b2.Property<string>("Path")
                                        .IsRequired()
                                        .HasMaxLength(150)
                                        .HasColumnType("character varying(150)");

                                    b2.HasKey("DetailPetId", "Id")
                                        .HasName("pk_pets");

                                    b2.ToTable("pets");

                                    b2.WithOwner()
                                        .HasForeignKey("DetailPetId")
                                        .HasConstraintName("fk_pets_pets_detail_pet_id");
                                });

                            b1.Navigation("Photos");

                            b1.Navigation("Requisites");
                        });

                    b.Navigation("Details")
                        .IsRequired();
                });

            modelBuilder.Entity("PetFamily.Domain.Models.Volunteers.Volunteer", b =>
                {
                    b.OwnsOne("PetFamily.Domain.Models.Volunteers.Detail", "Details", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("details");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetFamily.Domain.Models.Requisite", "Requisites", b2 =>
                                {
                                    b2.Property<Guid>("DetailVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(2000)
                                        .HasColumnType("character varying(2000)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)");

                                    b2.HasKey("DetailVolunteerId", "Id")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("DetailVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_detail_volunteer_id");
                                });

                            b1.OwnsMany("PetFamily.Domain.Models.Volunteers.SocialNetwork", "SocialNetworks", b2 =>
                                {
                                    b2.Property<Guid>("DetailVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Title")
                                        .IsRequired()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)");

                                    b2.Property<string>("Url")
                                        .IsRequired()
                                        .HasMaxLength(50)
                                        .HasColumnType("character varying(50)");

                                    b2.HasKey("DetailVolunteerId", "Id")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("DetailVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_detail_volunteer_id");
                                });

                            b1.Navigation("Requisites");

                            b1.Navigation("SocialNetworks");
                        });

                    b.Navigation("Details")
                        .IsRequired();
                });

            modelBuilder.Entity("PetFamily.Domain.Models.Volunteers.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
