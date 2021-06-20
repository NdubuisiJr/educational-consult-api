using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationalConsultAPI.Migrations
{
    public partial class addedGradeScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GreadeScale",
                table: "Schools",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GreadeScale",
                table: "Schools");
        }
    }
}
