﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Presentation;

#nullable disable

namespace Presentation.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Presentation.Entities.AppTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AssigneeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BoardId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ColumnId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("d640d006-403c-4fe5-bdb9-e4c7b1a8b1ee"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("ColumnId");

                    b.HasIndex("TeamId");

                    b.ToTable("Tasks", (string)null);
                });

            modelBuilder.Entity("Presentation.Entities.AppTeam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("1e2ad15b-7bdd-4b9c-96fa-ed60fefddd69"));

                    b.HasKey("Id");

                    b.ToTable("Teams", (string)null);
                });

            modelBuilder.Entity("Presentation.Entities.BoardColumn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("TaskLimit")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("Columns", (string)null);
                });

            modelBuilder.Entity("Presentation.Entities.TaskBoard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Boards", (string)null);
                });

            modelBuilder.Entity("Presentation.Entities.TeamMember", b =>
                {
                    b.Property<Guid>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.HasKey("MemberId", "TeamId");

                    b.ToTable("TeamMembers", (string)null);
                });

            modelBuilder.Entity("Presentation.Entities.AppTask", b =>
                {
                    b.HasOne("Presentation.Entities.TaskBoard", "Board")
                        .WithMany("Tasks")
                        .HasForeignKey("BoardId");

                    b.HasOne("Presentation.Entities.BoardColumn", "Column")
                        .WithMany("Tasks")
                        .HasForeignKey("ColumnId");

                    b.HasOne("Presentation.Entities.AppTeam", "Team")
                        .WithMany("Tasks")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("Column");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Presentation.Entities.BoardColumn", b =>
                {
                    b.HasOne("Presentation.Entities.TaskBoard", "Board")
                        .WithMany("Columns")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("Presentation.Entities.TaskBoard", b =>
                {
                    b.HasOne("Presentation.Entities.AppTeam", "Team")
                        .WithMany("Boards")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Presentation.Entities.AppTeam", b =>
                {
                    b.Navigation("Boards");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Presentation.Entities.BoardColumn", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Presentation.Entities.TaskBoard", b =>
                {
                    b.Navigation("Columns");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
