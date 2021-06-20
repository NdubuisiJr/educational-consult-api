﻿// <auto-generated />
using System;
using EducationalConsultAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EducationalConsultAPI.Migrations
{
    [DbContext(typeof(EducationalDbContext))]
    [Migration("20210513214848_addedGradeScale")]
    partial class addedGradeScale
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("EducationalConsultAPI.Models.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Average")
                        .HasColumnType("double precision");

                    b.Property<double>("CGPA")
                        .HasColumnType("double precision");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<Guid?>("DailyReportId")
                        .HasColumnType("uuid");

                    b.Property<string>("Grade")
                        .HasColumnType("text");

                    b.Property<double>("GradePoint")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("OverallScore")
                        .HasColumnType("double precision");

                    b.Property<string>("Rank")
                        .HasColumnType("text");

                    b.Property<string>("Remark")
                        .HasColumnType("text");

                    b.Property<double>("Score")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("DailyReportId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Class", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.HasIndex("UserId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.DailyReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Term")
                        .HasColumnType("text");

                    b.Property<DateTime>("TermEndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("TermStartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("DailyReports");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.InvitedUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("InvitedUsers");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClassId")
                        .HasColumnType("uuid");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.School", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<double>("GreadeScale")
                        .HasColumnType("double precision");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("OfficialEmail")
                        .HasColumnType("text");

                    b.Property<string>("OfficialPhone")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("OtherNames")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<int>("VerificationCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.UserGroup", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Student", b =>
                {
                    b.HasBaseType("EducationalConsultAPI.Models.User");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<byte>("Age")
                        .HasColumnType("smallint");

                    b.Property<Guid?>("ClassId")
                        .HasColumnType("uuid");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<string>("GuardianEmail")
                        .HasColumnType("text");

                    b.Property<string>("GuardianName")
                        .HasColumnType("text");

                    b.Property<string>("GuardianPhone")
                        .HasColumnType("text");

                    b.Property<string>("Rank")
                        .HasColumnType("text");

                    b.HasIndex("ClassId");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Activity", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.DailyReport", "DailyReport")
                        .WithMany("Activities")
                        .HasForeignKey("DailyReportId");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Class", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.School", "School")
                        .WithMany("Classes")
                        .HasForeignKey("SchoolId");

                    b.HasOne("EducationalConsultAPI.Models.User", "User")
                        .WithMany("Classes")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.DailyReport", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.Student", "Student")
                        .WithMany("DailyReports")
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Group", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.School", "School")
                        .WithMany("Groups")
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.InvitedUser", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.Group", "Group")
                        .WithMany("InvitedUsers")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Resource", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.Class", "Class")
                        .WithMany("Resources")
                        .HasForeignKey("ClassId");
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.UserGroup", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationalConsultAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationalConsultAPI.Models.Student", b =>
                {
                    b.HasOne("EducationalConsultAPI.Models.Class", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassId");
                });
#pragma warning restore 612, 618
        }
    }
}