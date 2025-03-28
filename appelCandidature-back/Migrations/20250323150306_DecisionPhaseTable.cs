using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class DecisionPhaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionPhase");

            migrationBuilder.DropColumn(
                name: "DateDebut",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "DateFin",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Phases");

            migrationBuilder.CreateTable(
                name: "DecisionPhases",
                columns: table => new
                {
                    DecisionId = table.Column<int>(type: "int", nullable: false),
                    PhaseId = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionPhases", x => new { x.DecisionId, x.PhaseId });
                    table.ForeignKey(
                        name: "FK_DecisionPhases_Decision_DecisionId",
                        column: x => x.DecisionId,
                        principalTable: "Decision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecisionPhases_Phases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DecisionPhases_PhaseId",
                table: "DecisionPhases",
                column: "PhaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionPhases");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebut",
                table: "Phases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFin",
                table: "Phases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "Phases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DecisionPhase",
                columns: table => new
                {
                    DecisionId = table.Column<int>(type: "int", nullable: false),
                    PhaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionPhase", x => new { x.DecisionId, x.PhaseId });
                    table.ForeignKey(
                        name: "FK_DecisionPhase_Decision_DecisionId",
                        column: x => x.DecisionId,
                        principalTable: "Decision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DecisionPhase_Phases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DecisionPhase_PhaseId",
                table: "DecisionPhase",
                column: "PhaseId");
        }
    }
}
