using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class updateEntityPoste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Supprimer la contrainte de clé étrangère
            migrationBuilder.DropForeignKey(
                name: "FK_Entites_Postes_PosteId",
                table: "Entites");

            // Supprimer l'index
            migrationBuilder.DropIndex(
                name: "IX_Entites_PosteId",
                table: "Entites");

            // Supprimer la colonne PosteId
            migrationBuilder.DropColumn(
                name: "PosteId",
                table: "Entites");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Ré-ajouter la colonne
            migrationBuilder.AddColumn<int>(
                name: "PosteId",
                table: "Entites",
                type: "int",
                nullable: true);

            // Ré-ajouter l'index
            migrationBuilder.CreateIndex(
                name: "IX_Entites_PosteId",
                table: "Entites",
                column: "PosteId");

            // Ré-ajouter la contrainte de clé étrangère
            migrationBuilder.AddForeignKey(
                name: "FK_Entites_Postes_PosteId",
                table: "Entites",
                column: "PosteId",
                principalTable: "Postes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

    }
}
