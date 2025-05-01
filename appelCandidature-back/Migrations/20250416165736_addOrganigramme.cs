using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class addOrganigramme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganigrammeId",
                table: "Poste",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Organigramme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosteOccupe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntiteId = table.Column<int>(type: "int", nullable: false),
                    TypePosteId = table.Column<int>(type: "int", nullable: false),
                    RattachementHierarchique = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localite = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organigramme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organigramme_Entites_EntiteId",
                        column: x => x.EntiteId,
                        principalTable: "Entites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organigramme_TypePostes_TypePosteId",
                        column: x => x.TypePosteId,
                        principalTable: "TypePostes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostesÀPostuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosteId = table.Column<int>(type: "int", nullable: false),
                    CandidatId = table.Column<int>(type: "int", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_PostesÀPostuler_Poste_PosteId",
                        column: x => x.PosteId,
                        principalTable: "Poste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poste_OrganigrammeId",
                table: "Poste",
                column: "OrganigrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_Organigramme_EntiteId",
                table: "Organigramme",
                column: "EntiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Organigramme_TypePosteId",
                table: "Organigramme",
                column: "TypePosteId");

            migrationBuilder.CreateIndex(
                name: "IX_PostesÀPostuler_CandidatId",
                table: "PostesÀPostuler",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_PostesÀPostuler_PosteId",
                table: "PostesÀPostuler",
                column: "PosteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Poste_Organigramme_OrganigrammeId",
                table: "Poste",
                column: "OrganigrammeId",
                principalTable: "Organigramme",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poste_Organigramme_OrganigrammeId",
                table: "Poste");

            migrationBuilder.DropTable(
                name: "Organigramme");

            migrationBuilder.DropTable(
                name: "PostesÀPostuler");

            migrationBuilder.DropIndex(
                name: "IX_Poste_OrganigrammeId",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "OrganigrammeId",
                table: "Poste");
        }
    }
}
