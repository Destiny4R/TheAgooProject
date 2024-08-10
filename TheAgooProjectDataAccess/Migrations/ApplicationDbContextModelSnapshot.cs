﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheAgooProjectDataAccess.Data;

#nullable disable

namespace TheAgooProjectDataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("varchar(21)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TheAgooProjectModel.GeneralClassTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("ClassTeacher")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ExamOfficerID")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("NextTermStart")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Next_Term_Fees")
                        .HasColumnType("double");

                    b.Property<int>("SessionYearId")
                        .HasColumnType("int");

                    b.Property<int>("SubClassId")
                        .HasColumnType("int");

                    b.Property<string>("Term")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("TermEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("TotalAttendance")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("SessionYearId");

                    b.HasIndex("SubClassId");

                    b.ToTable("GeneralClassTables");
                });

            modelBuilder.Entity("TheAgooProjectModel.RemarkPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Absent")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("AddedBy")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("General_Remark")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<bool>("P_Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("Position_In_Class")
                        .HasColumnType("int");

                    b.Property<string>("Principal_Remark")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<bool>("R_Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<double?>("Student_Attendance")
                        .HasColumnType("double");

                    b.Property<int>("TermRegId")
                        .HasColumnType("int");

                    b.Property<double?>("Total_Marks_Obtain")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("TermRegId")
                        .IsUnique();

                    b.ToTable("RemarkPositions");
                });

            modelBuilder.Entity("TheAgooProjectModel.ResultTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Assignment")
                        .HasColumnType("double");

                    b.Property<double?>("ClassWork")
                        .HasColumnType("double");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<double?>("Examination")
                        .HasColumnType("double");

                    b.Property<string>("ExamsOfficer")
                        .HasColumnType("longtext");

                    b.Property<string>("Grade")
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<int?>("Position")
                        .HasColumnType("int");

                    b.Property<double?>("Project")
                        .HasColumnType("double");

                    b.Property<string>("Remark")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("TermRegId")
                        .HasColumnType("int");

                    b.Property<double?>("Test")
                        .HasColumnType("double");

                    b.Property<double?>("Total")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TermRegId");

                    b.ToTable("ResultTable");
                });

            modelBuilder.Entity("TheAgooProjectModel.SchoolClasses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Elective")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("SchoolClasses");
                });

            modelBuilder.Entity("TheAgooProjectModel.SessionYear", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("SessionYears");
                });

            modelBuilder.Entity("TheAgooProjectModel.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .HasMaxLength(452)
                        .HasColumnType("varchar(452)");

                    b.Property<bool>("CanPrintResult")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("CashierSignature")
                        .HasColumnType("longtext");

                    b.Property<int?>("Classes")
                        .HasColumnType("int");

                    b.Property<string>("PrincipalName")
                        .HasColumnType("longtext");

                    b.Property<string>("PrincipalSignature")
                        .HasColumnType("longtext");

                    b.Property<string>("Term")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("sessionYear")
                        .HasColumnType("int");

                    b.Property<int?>("subclass")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("TheAgooProjectModel.StudentRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte?>("Attendance")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Attentiveness")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Constrution")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Courage")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Curiosity")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Drawing_Painting")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Fluecy")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Games_Sport")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Handing_WShop_Tool")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Handwriting")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Honesty")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Humility")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Industry")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Initiative")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Leadership")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Musical_Skill")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Neatness")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Organisational_Ability")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Perseverance")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Politeness")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("Punctuality")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Relation_With_Staff")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Relationship_With_Student")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Reliability")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Respect_For_Other")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("SelfControl")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Sense_of_Responsibility")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte?>("Spirit_of_Cooperation")
                        .HasColumnType("tinyint unsigned");

                    b.Property<int>("TermRegId")
                        .HasColumnType("int");

                    b.Property<byte?>("Tolanrance")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("TermRegId")
                        .IsUnique();

                    b.ToTable("StudentRatings");
                });

            modelBuilder.Entity("TheAgooProjectModel.StudentsData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LocalGov")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("OtherName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Passport")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("SessionYearId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("StudentRegNo")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("SessionYearId");

                    b.ToTable("StudentTable");
                });

            modelBuilder.Entity("TheAgooProjectModel.SubClasses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("SubClasses");
                });

            modelBuilder.Entity("TheAgooProjectModel.Subjects", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("TheAgooProjectModel.TermRegistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassesInSchoolId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("ResultIsReady")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SessionYearId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasMaxLength(250)
                        .HasColumnType("int");

                    b.Property<int>("SubClassId")
                        .HasColumnType("int");

                    b.Property<string>("Term")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("ClassesInSchoolId");

                    b.HasIndex("SessionYearId");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubClassId");

                    b.ToTable("TermRegistrations");
                });

            modelBuilder.Entity("TheAgooProjectModel.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("varchar(80)");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheAgooProjectModel.GeneralClassTable", b =>
                {
                    b.HasOne("TheAgooProjectModel.SchoolClasses", "Classcchool")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheAgooProjectModel.SessionYear", "SessionYear")
                        .WithMany()
                        .HasForeignKey("SessionYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheAgooProjectModel.SubClasses", "SubClasses")
                        .WithMany()
                        .HasForeignKey("SubClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classcchool");

                    b.Navigation("SessionYear");

                    b.Navigation("SubClasses");
                });

            modelBuilder.Entity("TheAgooProjectModel.RemarkPosition", b =>
                {
                    b.HasOne("TheAgooProjectModel.TermRegistration", "Termregistration")
                        .WithOne("RemarkPositions")
                        .HasForeignKey("TheAgooProjectModel.RemarkPosition", "TermRegId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Termregistration");
                });

            modelBuilder.Entity("TheAgooProjectModel.ResultTable", b =>
                {
                    b.HasOne("TheAgooProjectModel.Subjects", "Subjects")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheAgooProjectModel.TermRegistration", "Termregistration")
                        .WithMany("ResultTable")
                        .HasForeignKey("TermRegId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subjects");

                    b.Navigation("Termregistration");
                });

            modelBuilder.Entity("TheAgooProjectModel.Settings", b =>
                {
                    b.HasOne("TheAgooProjectModel.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("TheAgooProjectModel.StudentRating", b =>
                {
                    b.HasOne("TheAgooProjectModel.TermRegistration", "Termregistration")
                        .WithOne("StudentRatings")
                        .HasForeignKey("TheAgooProjectModel.StudentRating", "TermRegId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Termregistration");
                });

            modelBuilder.Entity("TheAgooProjectModel.StudentsData", b =>
                {
                    b.HasOne("TheAgooProjectModel.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheAgooProjectModel.SessionYear", "SessionYear")
                        .WithMany()
                        .HasForeignKey("SessionYearId");

                    b.Navigation("ApplicationUser");

                    b.Navigation("SessionYear");
                });

            modelBuilder.Entity("TheAgooProjectModel.TermRegistration", b =>
                {
                    b.HasOne("TheAgooProjectModel.SchoolClasses", "Schoolclasses")
                        .WithMany()
                        .HasForeignKey("ClassesInSchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheAgooProjectModel.SessionYear", "SessionYear")
                        .WithMany()
                        .HasForeignKey("SessionYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheAgooProjectModel.StudentsData", "StudentsData")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheAgooProjectModel.SubClasses", "SubClasses")
                        .WithMany()
                        .HasForeignKey("SubClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schoolclasses");

                    b.Navigation("SessionYear");

                    b.Navigation("StudentsData");

                    b.Navigation("SubClasses");
                });

            modelBuilder.Entity("TheAgooProjectModel.TermRegistration", b =>
                {
                    b.Navigation("RemarkPositions")
                        .IsRequired();

                    b.Navigation("ResultTable");

                    b.Navigation("StudentRatings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
