using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class DecisionPhaseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Phases_Decision_DecisionId",
                table: "Phases");

            migrationBuilder.DropIndex(
                name: "IX_Phases_DecisionId",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "DecisionId",
                table: "Phases");

            migrationBuilder.AlterColumn<int>(
                name: "Nom",
                table: "Phases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Statut",
                table: "Decision",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionPhase");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Phases",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DecisionId",
                table: "Phases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Statut",
                table: "Decision",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phases_DecisionId",
                table: "Phases",
                column: "DecisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Phases_Decision_DecisionId",
                table: "Phases",
                column: "DecisionId",
                principalTable: "Decision",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
