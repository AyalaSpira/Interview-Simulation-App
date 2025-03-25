using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewSim.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "SummaryPath",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummaryPath",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Resume",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
