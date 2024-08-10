using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheAgooProjectDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingApplicationResultNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermRegistrations_AspNetUsers_ApplicationUserID",
                table: "TermRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TermRegistrations_ApplicationUserID",
                table: "TermRegistrations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "TermRegistrations");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "TermRegistrations");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "TermRegistrations",
                type: "int",
                maxLength: 250,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Total",
                table: "ResultTable",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "Test",
                table: "ResultTable",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "ResultTable",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "Project",
                table: "ResultTable",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "ResultTable",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "ResultTable",
                type: "varchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldMaxLength: 4)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "Examination",
                table: "ResultTable",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AlterColumn<double>(
                name: "Assignment",
                table: "ResultTable",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.CreateIndex(
                name: "IX_TermRegistrations_StudentId",
                table: "TermRegistrations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TermRegistrations_StudentTable_StudentId",
                table: "TermRegistrations",
                column: "StudentId",
                principalTable: "StudentTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermRegistrations_StudentTable_StudentId",
                table: "TermRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TermRegistrations_StudentId",
                table: "TermRegistrations");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "TermRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "TermRegistrations",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "TermRegistrations",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "Total",
                table: "ResultTable",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Test",
                table: "ResultTable",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ResultTable",
                keyColumn: "Remark",
                keyValue: null,
                column: "Remark",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "ResultTable",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "Project",
                table: "ResultTable",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "ResultTable",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ResultTable",
                keyColumn: "Grade",
                keyValue: null,
                column: "Grade",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "ResultTable",
                type: "varchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldMaxLength: 4,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<double>(
                name: "Examination",
                table: "ResultTable",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Assignment",
                table: "ResultTable",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TermRegistrations_ApplicationUserID",
                table: "TermRegistrations",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_TermRegistrations_AspNetUsers_ApplicationUserID",
                table: "TermRegistrations",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
