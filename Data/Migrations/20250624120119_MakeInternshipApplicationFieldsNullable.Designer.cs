﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250624120119_MakeInternshipApplicationFieldsNullable")]
    partial class MakeInternshipApplicationFieldsNullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Advisor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Advisors");
                });

            modelBuilder.Entity("Core.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UniversityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Core.Entities.InternshipApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdvisorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("InternshipPlaceId")
                        .HasColumnType("int");

                    b.Property<string>("InternshipTopic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RejectionReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("TotalDays")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdvisorId");

                    b.HasIndex("InternshipPlaceId");

                    b.HasIndex("StudentId");

                    b.ToTable("InternshipApplications");
                });

            modelBuilder.Entity("Core.Entities.InternshipDiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<int?>("ApprovedByAdvisorId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int>("InternshipApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("RejectionReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<string>("WorkDone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedByAdvisorId");

                    b.HasIndex("InternshipApplicationId");

                    b.ToTable("InternshipDiaries");
                });

            modelBuilder.Entity("Core.Entities.InternshipEvaluation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdvisorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EvaluationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GeneralEvaluation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InternshipApplicationId")
                        .HasColumnType("int");

                    b.Property<int>("LearningAbility")
                        .HasColumnType("int");

                    b.Property<int>("ProblemSolving")
                        .HasColumnType("int");

                    b.Property<bool>("SuccessStatus")
                        .HasColumnType("bit");

                    b.Property<int>("Teamwork")
                        .HasColumnType("int");

                    b.Property<int>("TimeManagement")
                        .HasColumnType("int");

                    b.Property<int>("WorkPerformance")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdvisorId");

                    b.HasIndex("InternshipApplicationId")
                        .IsUnique();

                    b.ToTable("InternshipEvaluations");
                });

            modelBuilder.Entity("Core.Entities.InternshipPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkArea")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InternshipPlaces");
                });

            modelBuilder.Entity("Core.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AdvisorId")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdvisorId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Core.Entities.University", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Universities");
                });

            modelBuilder.Entity("Core.Entities.Advisor", b =>
                {
                    b.HasOne("Core.Entities.Department", "Department")
                        .WithMany("Advisors")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Core.Entities.Department", b =>
                {
                    b.HasOne("Core.Entities.University", "University")
                        .WithMany("Departments")
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("Core.Entities.InternshipApplication", b =>
                {
                    b.HasOne("Core.Entities.Advisor", "Advisor")
                        .WithMany("InternshipApplications")
                        .HasForeignKey("AdvisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.InternshipPlace", "InternshipPlace")
                        .WithMany("InternshipApplications")
                        .HasForeignKey("InternshipPlaceId");

                    b.HasOne("Core.Entities.Student", "Student")
                        .WithMany("InternshipApplications")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advisor");

                    b.Navigation("InternshipPlace");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Core.Entities.InternshipDiary", b =>
                {
                    b.HasOne("Core.Entities.Advisor", "ApprovedByAdvisor")
                        .WithMany()
                        .HasForeignKey("ApprovedByAdvisorId");

                    b.HasOne("Core.Entities.InternshipApplication", "InternshipApplication")
                        .WithMany("InternshipDiaries")
                        .HasForeignKey("InternshipApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovedByAdvisor");

                    b.Navigation("InternshipApplication");
                });

            modelBuilder.Entity("Core.Entities.InternshipEvaluation", b =>
                {
                    b.HasOne("Core.Entities.Advisor", "Advisor")
                        .WithMany()
                        .HasForeignKey("AdvisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.InternshipApplication", "InternshipApplication")
                        .WithOne("Evaluation")
                        .HasForeignKey("Core.Entities.InternshipEvaluation", "InternshipApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advisor");

                    b.Navigation("InternshipApplication");
                });

            modelBuilder.Entity("Core.Entities.Student", b =>
                {
                    b.HasOne("Core.Entities.Advisor", "Advisor")
                        .WithMany("Students")
                        .HasForeignKey("AdvisorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Core.Entities.Department", "Department")
                        .WithMany("Students")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Advisor");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Core.Entities.Advisor", b =>
                {
                    b.Navigation("InternshipApplications");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Core.Entities.Department", b =>
                {
                    b.Navigation("Advisors");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Core.Entities.InternshipApplication", b =>
                {
                    b.Navigation("Evaluation")
                        .IsRequired();

                    b.Navigation("InternshipDiaries");
                });

            modelBuilder.Entity("Core.Entities.InternshipPlace", b =>
                {
                    b.Navigation("InternshipApplications");
                });

            modelBuilder.Entity("Core.Entities.Student", b =>
                {
                    b.Navigation("InternshipApplications");
                });

            modelBuilder.Entity("Core.Entities.University", b =>
                {
                    b.Navigation("Departments");
                });
#pragma warning restore 612, 618
        }
    }
}
