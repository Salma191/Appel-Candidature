using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class addEntityPosteRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntiteId",
                table: "Poste",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntiteId",
                table: "Entites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Poste_EntiteId",
                table: "Poste",
                column: "EntiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Entites_EntiteId",
                table: "Entites",
                column: "EntiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entites_Entites_EntiteId",
                table: "Entites",
                column: "EntiteId",
                principalTable: "Entites",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Poste_Entites_EntiteId",
                table: "Poste",
                column: "EntiteId",
                principalTable: "Entites",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entites_Entites_EntiteId",
                table: "Entites");

            migrationBuilder.DropForeignKey(
                name: "FK_Poste_Entites_EntiteId",
                table: "Poste");

            migrationBuilder.DropIndex(
                name: "IX_Poste_EntiteId",
                table: "Poste");

            migrationBuilder.DropIndex(
                name: "IX_Entites_EntiteId",
                table: "Entites");

            migrationBuilder.DropColumn(
                name: "EntiteId",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "EntiteId",
                table: "Entites");
        }
    }
}
