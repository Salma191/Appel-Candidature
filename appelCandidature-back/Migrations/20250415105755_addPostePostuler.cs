using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class addPostePostuler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostesÀPostuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosteId = table.Column<int>(type: "int", nullable: false),
                    CandidatId = table.Column<int>(type: "int", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "datetime2", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostesÀPostuler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostesÀPostuler_Candidats_CandidatId",
                        column: x => x.CandidatId,
                        principalTable: "Candidats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostesÀPostuler_Postes_PosteId",
                        column: x => x.PosteId,
                        principalTable: "Poste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostesÀPostuler_CandidatId",
                table: "PostesÀPostuler",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_PostesÀPostuler_PosteId",
                table: "PostesÀPostuler",
                column: "PosteId");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostesÀPostuler");
        }

    }
}
