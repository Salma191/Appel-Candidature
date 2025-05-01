using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class addCandidatureDecisionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DecisionId",
                table: "Candidatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_DecisionId",
                table: "Candidatures",
                column: "DecisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidatures_Decision_DecisionId",
                table: "Candidatures",
                column: "DecisionId",
                principalTable: "Decision",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidatures_Decision_DecisionId",
                table: "Candidatures");

            migrationBuilder.DropIndex(
                name: "IX_Candidatures_DecisionId",
                table: "Candidatures");

            migrationBuilder.DropColumn(
                name: "DecisionId",
                table: "Candidatures");
        }
    }
}
