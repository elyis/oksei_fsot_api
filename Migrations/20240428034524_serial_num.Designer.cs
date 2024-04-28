﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using oksei_fsot_api.src.Infrastructure.Data;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240428034524_serial_num")]
    partial class serial_num
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.CriterionEvaluationOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("CountPoints")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("CriterionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CriterionId");

                    b.ToTable("CriterionEvaluationOptions");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.CriterionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Criterions");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.EvaluatedAppraiserModel", b =>
                {
                    b.Property<Guid>("EvaluatedId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AppraiserId")
                        .HasColumnType("TEXT");

                    b.HasKey("EvaluatedId", "AppraiserId");

                    b.HasIndex("AppraiserId");

                    b.ToTable("EvaluatedAppraisers");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.MarkLogModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MarkId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MarkId")
                        .IsUnique();

                    b.ToTable("MarkLogs");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.MarkModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CriterionId")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EvaluatedAppraiserAppraiserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EvaluatedAppraiserEvaluatedId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CriterionId");

                    b.HasIndex("EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.PremiumReportModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("CostByPoint")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("DistributablePremium")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<int>("FixedPremium")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PartSemiannualPremium")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalAmountPoints")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalAmountPremium")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PremiumReports");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.ReportTeacherModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("CountPoints")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<float>("Premium")
                        .HasColumnType("REAL");

                    b.Property<Guid>("PremiumReportId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PremiumReportId");

                    b.HasIndex("UserId");

                    b.ToTable("ReportTeachers");
                });

            modelBuilder.Entity("oksei_fsot_api.src.Domain.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("LastEvaluationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("TokenValidBefore")
                        .HasColumnType("TEXT");

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
                    b.HasOne("oksei_fsot_api.src.Domain.Models.CriterionModel", "Criterion")
                        .WithMany("Marks")
                        .HasForeignKey("CriterionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("oksei_fsot_api.src.Domain.Models.EvaluatedAppraiserModel", "EvaluatedAppraiser")
                        .WithMany("Marks")
                        .HasForeignKey("EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Criterion");

                    b.Navigation("EvaluatedAppraiser");
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