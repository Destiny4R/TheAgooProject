using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheAgooProjectDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRemarkPositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_RemarkPositions_TermRegId",
            //    table: "RemarkPositions");

            //migrationBuilder.AddColumn<bool>(
            //    name: "R_Status",
            //    table: "RemarkPositions",
            //    type: "tinyint(1)",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.CreateIndex(
            //    name: "IX_RemarkPositions_TermRegId",
            //    table: "RemarkPositions",
            //    column: "TermRegId",
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_RemarkPositions_TermRegId",
            //    table: "RemarkPositions");

            //migrationBuilder.DropColumn(
            //    name: "R_Status",
            //    table: "RemarkPositions");

            migrationBuilder.CreateIndex(
                name: "IX_RemarkPositions_TermRegId",
                table: "RemarkPositions",
                column: "TermRegId");
        }
    }
}
