using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class AddUtilisateurIdMmbre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_MembreCommissions_MembreCommissionId",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_MembreCommissionId",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "MembreCommissionId",
                table: "Utilisateurs");

            migrationBuilder.AddColumn<int>(
                name: "UtilisateurId",
                table: "MembreCommissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MembreCommissions_UtilisateurId",
                table: "MembreCommissions",
                column: "UtilisateurId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MembreCommissions_Utilisateurs_UtilisateurId",
                table: "MembreCommissions",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembreCommissions_Utilisateurs_UtilisateurId",
                table: "MembreCommissions");

            migrationBuilder.DropIndex(
                name: "IX_MembreCommissions_UtilisateurId",
                table: "MembreCommissions");

            migrationBuilder.DropColumn(
                name: "UtilisateurId",
                table: "MembreCommissions");

            migrationBuilder.AddColumn<int>(
                name: "MembreCommissionId",
                table: "Utilisateurs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_MembreCommissionId",
                table: "Utilisateurs",
                column: "MembreCommissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_MembreCommissions_MembreCommissionId",
                table: "Utilisateurs",
                column: "MembreCommissionId",
                principalTable: "MembreCommissions",
                principalColumn: "Id");
        }
    }
}
