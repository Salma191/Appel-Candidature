using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIndexOnUtilisateurId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MembreCommissions_UtilisateurId",
                table: "MembreCommissions"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MembreCommissions_UtilisateurId",
                table: "MembreCommissions",
                column: "UtilisateurId",
                unique: true // Pour restaurer l'index unique si tu fais un rollback
            );
        }
    }
}
