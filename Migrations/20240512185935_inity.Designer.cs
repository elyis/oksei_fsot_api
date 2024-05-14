﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using oksei_fsot_api.src.Infrastructure.Data;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240512185935_inity")]
    partial class inity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.CriterionEvaluationOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CountPoints")
                        .HasColumnType("integer");

                    b.Property<Guid>("CriterionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CriterionId");

                    b.ToTable("CriterionEvaluationOptions");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.CriterionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Criterions");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.EvaluatedAppraiserModel", b =>
                {
                    b.Property<Guid>("EvaluatedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppraiserId")
                        .HasColumnType("uuid");

                    b.HasKey("EvaluatedId", "AppraiserId");

                    b.HasIndex("AppraiserId");

                    b.ToTable("EvaluatedAppraisers");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.MarkLogModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("MarkId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MarkId")
                        .IsUnique();

                    b.ToTable("MarkLogs");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.MarkModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CriterionModelId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EvaluatedAppraiserAppraiserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EvaluatedAppraiserEvaluatedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EvaluationOptionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CriterionModelId");

                    b.HasIndex("EvaluationOptionId");

                    b.HasIndex("EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.PremiumReportModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CostByPoint")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("DistributablePremium")
                        .HasColumnType("integer");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<int>("FixedPremium")
                        .HasColumnType("integer");

                    b.Property<int>("PartSemiannualPremium")
                        .HasColumnType("integer");

                    b.Property<int>("TotalAmountPoints")
                        .HasColumnType("integer");

                    b.Property<int>("TotalAmountPremium")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PremiumReports");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.ReportTeacherModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CountPoints")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<float>("Premium")
                        .HasColumnType("real");

                    b.Property<Guid>("PremiumReportId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PremiumReportId");

                    b.HasIndex("UserId");

                    b.ToTable("ReportTeachers");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("boolean");

                    b.Property<DateOnly?>("LastEvaluationDate")
                        .HasColumnType("date");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TokenValidBefore")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("Token");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.CriterionEvaluationOption", b =>
                {
                    b.HasOne("oksei_fsot_api.src.Domain.Models.CriterionModel", "Criterion")
                        .WithMany("EvaluationOptions")
                        .HasForeignKey("CriterionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Criterion");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.EvaluatedAppraiserModel", b =>
                {
                    b.HasOne("oksei_fsot_api.src.Domain.Models.UserModel", "Appraiser")
                        .WithMany()
                        .HasForeignKey("AppraiserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("oksei_fsot_api.src.Domain.Models.UserModel", "Evaluated")
                        .WithMany("UserAppraisers")
                        .HasForeignKey("EvaluatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appraiser");

                    b.Navigation("Evaluated");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.MarkLogModel", b =>
                {
                    b.HasOne("oksei_fsot_api.src.Domain.Models.MarkModel", "Mark")
                        .WithOne("MarkLogs")
                        .HasForeignKey("oksei_fsot_api.src.Domain.Models.MarkLogModel", "MarkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mark");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.MarkModel", b =>
                {
                    b.HasOne("oksei_fsot_api.src.Domain.Models.CriterionModel", null)
                        .WithMany("Marks")
                        .HasForeignKey("CriterionModelId");

                    b.HasOne("oksei_fsot_api.src.Domain.Models.CriterionEvaluationOption", "EvaluationOption")
                        .WithMany()
                        .HasForeignKey("EvaluationOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("oksei_fsot_api.src.Domain.Models.EvaluatedAppraiserModel", "EvaluatedAppraiser")
                        .WithMany("Marks")
                        .HasForeignKey("EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EvaluatedAppraiser");

                    b.Navigation("EvaluationOption");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.ReportTeacherModel", b =>
                {
                    b.HasOne("oksei_fsot_api.src.Domain.Models.PremiumReportModel", "PremiumReport")
                        .WithMany("ReportTeachers")
                        .HasForeignKey("PremiumReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("oksei_fsot_api.src.Domain.Models.UserModel", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PremiumReport");

                    b.Navigation("User");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.CriterionModel", b =>
                {
                    b.Navigation("EvaluationOptions");

                    b.Navigation("Marks");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.EvaluatedAppraiserModel", b =>
                {
                    b.Navigation("Marks");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.MarkModel", b =>
                {
                    b.Navigation("MarkLogs")
                        .IsRequired();
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.PremiumReportModel", b =>
                {
                    b.Navigation("ReportTeachers");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.UserModel", b =>
                {
                    b.Navigation("Reports");

                    b.Navigation("UserAppraisers");
                });
#pragma warning restore 612, 618
        }
    }
}