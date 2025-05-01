using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class addCandidatCritere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassementMinimum",
                table: "TypePostes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NbrExperienceMinimum",
                table: "TypePostes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NoteMinimum",
                table: "TypePostes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NbrExperience",
                table: "Candidats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Note",
                table: "Candidats",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassementMinimum",
                table: "TypePostes");

            migrationBuilder.DropColumn(
                name: "NbrExperienceMinimum",
                table: "TypePostes");

            migrationBuilder.DropColumn(
                name: "NoteMinimum",
                table: "TypePostes");

            migrationBuilder.DropColumn(
                name: "NbrExperience",
                table: "Candidats");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Candidats");
        }
    }
}
