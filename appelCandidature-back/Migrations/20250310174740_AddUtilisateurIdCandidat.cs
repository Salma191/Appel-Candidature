using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class AddUtilisateurIdCandidat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Candidats_CandidatId",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_CandidatId",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "CandidatId",
                table: "Utilisateurs");

            migrationBuilder.AddColumn<int>(
                name: "UtilisateurId",
                table: "Candidats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Candidats_UtilisateurId",
                table: "Candidats",
                column: "UtilisateurId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidats_Utilisateurs_UtilisateurId",
                table: "Candidats",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidats_Utilisateurs_UtilisateurId",
                table: "Candidats");

            migrationBuilder.DropIndex(
                name: "IX_Candidats_UtilisateurId",
                table: "Candidats");

            migrationBuilder.DropColumn(
                name: "UtilisateurId",
                table: "Candidats");

            migrationBuilder.AddColumn<int>(
                name: "CandidatId",
                table: "Utilisateurs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_CandidatId",
                table: "Utilisateurs",
                column: "CandidatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Candidats_CandidatId",
                table: "Utilisateurs",
                column: "CandidatId",
                principalTable: "Candidats",
                principalColumn: "Id");
        }
    }
}
