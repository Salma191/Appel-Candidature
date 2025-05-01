using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class addCandidaturePJ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "CandidatureId",
                table: "PieceJointes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PieceJointes_CandidatureId",
                table: "PieceJointes",
                column: "CandidatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_PieceJointes_Candidatures_CandidatureId",
                table: "PieceJointes",
                column: "CandidatureId",
                principalTable: "Candidatures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PieceJointes_Candidatures_CandidatureId",
                table: "PieceJointes");

            migrationBuilder.DropIndex(
                name: "IX_PieceJointes_CandidatureId",
                table: "PieceJointes");

            migrationBuilder.DropColumn(
                name: "CandidatureId",
                table: "PieceJointes");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidatureId = table.Column<int>(type: "int", nullable: false),
                    Lien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Candidatures_CandidatureId",
                        column: x => x.CandidatureId,
                        principalTable: "Candidatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CandidatureId",
                table: "Documents",
                column: "CandidatureId");
        }
    }
}
