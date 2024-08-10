using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheAgooProjectDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ResultTableAdding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Assignment = table.Column<double>(type: "double", nullable: false),
                    Test = table.Column<double>(type: "double", nullable: false),
                    Project = table.Column<double>(type: "double", nullable: false),
                    Examination = table.Column<double>(type: "double", nullable: false),
                    Total = table.Column<double>(type: "double", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TermRegId = table.Column<int>(type: "int", nullable: false),
                    ExamsOfficer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultTable_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultTable_TermRegistrations_TermRegId",
                        column: x => x.TermRegId,
                        principalTable: "TermRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTable_SubjectId",
                table: "ResultTable",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultTable_TermRegId",
                table: "ResultTable",
                column: "TermRegId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultTable");
        }
    }
}
