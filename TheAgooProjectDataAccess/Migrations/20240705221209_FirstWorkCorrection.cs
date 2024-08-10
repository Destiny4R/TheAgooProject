using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheAgooProjectDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FirstWorkCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "StudentTable",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTable_ApplicationUserId",
                table: "StudentTable",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTable_AspNetUsers_ApplicationUserId",
                table: "StudentTable",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTable_AspNetUsers_ApplicationUserId",
                table: "StudentTable");

            migrationBuilder.DropIndex(
                name: "IX_StudentTable_ApplicationUserId",
                table: "StudentTable");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "StudentTable");
        }
    }
}
