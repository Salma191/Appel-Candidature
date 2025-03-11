using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembreCommissionIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MembreCommissions_UtilisateurId_CommissionId",
                table: "MembreCommissions",
                columns: new[] { "UtilisateurId", "CommissionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MembreCommissions_UtilisateurId_CommissionId",
                table: "MembreCommissions");
        }
    }
}
