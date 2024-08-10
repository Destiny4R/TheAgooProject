using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheAgooProjectDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RatingPostionRemarkTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HeadTeacherSign",
                table: "Settings",
                newName: "PrincipalSignature");

            migrationBuilder.AddColumn<string>(
                name: "PrincipalName",
                table: "Settings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GeneralClassTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Term = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionYearId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    SubClassId = table.Column<int>(type: "int", nullable: false),
                    Next_Term_Fees = table.Column<double>(type: "double", nullable: false),
                    TermEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextTermStart = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClassTeacher = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalAttendance = table.Column<double>(type: "double", nullable: false),
                    ExamOfficerID = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralClassTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralClassTables_SchoolClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "SchoolClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralClassTables_SessionYears_SessionYearId",
                        column: x => x.SessionYearId,
                        principalTable: "SessionYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralClassTables_SubClasses_SubClassId",
                        column: x => x.SubClassId,
                        principalTable: "SubClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RemarkPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Student_Attendance = table.Column<double>(type: "double", nullable: true),
                    Absent = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Total_Marks_Obtain = table.Column<double>(type: "double", nullable: true),
                    Position_In_Class = table.Column<int>(type: "int", maxLength: 10, nullable: true),
                    General_Remark = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Principal_Remark = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddedBy = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    P_Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TermRegId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemarkPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemarkPositions_TermRegistrations_TermRegId",
                        column: x => x.TermRegId,
                        principalTable: "TermRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Attentiveness = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Attendance = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Reliability = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Punctuality = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Perseverance = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Neatness = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Sense_of_Responsibility = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Politeness = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Spirit_of_Cooperation = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    SelfControl = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Relationship_With_Student = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Relation_With_Staff = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Curiosity = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Initiative = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Honesty = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Industry = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Humility = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Organisational_Ability = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Tolanrance = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Leadership = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Respect_For_Other = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Courage = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Handwriting = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Fluecy = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Drawing_Painting = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Handing_WShop_Tool = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Games_Sport = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Musical_Skill = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    Constrution = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    TermRegId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRatings_TermRegistrations_TermRegId",
                        column: x => x.TermRegId,
                        principalTable: "TermRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralClassTables_ClassId",
                table: "GeneralClassTables",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralClassTables_SessionYearId",
                table: "GeneralClassTables",
                column: "SessionYearId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralClassTables_SubClassId",
                table: "GeneralClassTables",
                column: "SubClassId");

            migrationBuilder.CreateIndex(
                name: "IX_RemarkPositions_TermRegId",
                table: "RemarkPositions",
                column: "TermRegId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRatings_TermRegId",
                table: "StudentRatings",
                column: "TermRegId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralClassTables");

            migrationBuilder.DropTable(
                name: "RemarkPositions");

            migrationBuilder.DropTable(
                name: "StudentRatings");

            migrationBuilder.DropColumn(
                name: "PrincipalName",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "PrincipalSignature",
                table: "Settings",
                newName: "HeadTeacherSign");
        }
    }
}
