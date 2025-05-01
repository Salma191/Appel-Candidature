using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePJ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeDoc",
                table: "PieceJointes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstComplete",
                table: "Candidatures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeDoc",
                table: "PieceJointes");

            migrationBuilder.DropColumn(
                name: "EstComplete",
                table: "Candidatures");
        }
    }
}
