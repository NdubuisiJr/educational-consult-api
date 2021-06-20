using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationalConsultAPI.Migrations
{
    public partial class movedCGPAToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CGPA",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Activities");

            migrationBuilder.AddColumn<double>(
                name: "CGPA",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CGPA",
                table: "Users");

            migrationBuilder.AddColumn<double>(
                name: "CGPA",
                table: "Activities",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Activities",
                type: "text",
                nullable: true);
        }
    }
}
