using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheAgooProjectDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FirstWorkCorrectionAddingIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("ALTER TABLE Subjects AUTO_INCREMENT = 100;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
