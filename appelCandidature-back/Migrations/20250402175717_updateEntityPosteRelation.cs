using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class updateEntityPosteRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entites_Entites_EntiteId",
                table: "Entites");

            migrationBuilder.DropIndex(
                name: "IX_Entites_EntiteId",
                table: "Entites");

            migrationBuilder.DropColumn(
                name: "EntiteId",
                table: "Entites");

            migrationBuilder.AddColumn<int>(
                name: "PosteId",
                table: "Entites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entites_PosteId",
                table: "Entites",
                column: "PosteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entites_Postes_PosteId",
                table: "Entites",
                column: "PosteId",
                principalTable: "Poste",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntiteId",
                table: "Entites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entites_EntiteId",
                table: "Entites",
                column: "EntiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entites_Entites_EntiteId",
                table: "Entites",
                column: "EntiteId",
                principalTable: "Entites",
                principalColumn: "Id");
        }
    }
}
