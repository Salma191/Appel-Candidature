using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrganigramme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organigramme_Entites_EntiteId",
                table: "Organigramme");

            migrationBuilder.DropForeignKey(
                name: "FK_Organigramme_TypePostes_TypePosteId",
                table: "Organigramme");

            migrationBuilder.DropIndex(
                name: "IX_Organigramme_EntiteId",
                table: "Organigramme");

            migrationBuilder.DropIndex(
                name: "IX_Organigramme_TypePosteId",
                table: "Organigramme");

            migrationBuilder.DropColumn(
                name: "EntiteId",
                table: "Organigramme");

            migrationBuilder.DropColumn(
                name: "TypePosteId",
                table: "Organigramme");

            migrationBuilder.RenameColumn(
                name: "PosteOccupe",
                table: "Organigramme",
                newName: "TypePoste");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateNomination",
                table: "Organigramme",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Poste",
                table: "Organigramme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sigle",
                table: "Organigramme",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Titulaire",
                table: "Organigramme",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateNomination",
                table: "Organigramme");

            migrationBuilder.DropColumn(
                name: "Poste",
                table: "Organigramme");

            migrationBuilder.DropColumn(
                name: "Sigle",
                table: "Organigramme");

            migrationBuilder.DropColumn(
                name: "Titulaire",
                table: "Organigramme");

            migrationBuilder.RenameColumn(
                name: "TypePoste",
                table: "Organigramme",
                newName: "PosteOccupe");

            migrationBuilder.AddColumn<int>(
                name: "EntiteId",
                table: "Organigramme",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypePosteId",
                table: "Organigramme",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Organigramme_EntiteId",
                table: "Organigramme",
                column: "EntiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Organigramme_TypePosteId",
                table: "Organigramme",
                column: "TypePosteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organigramme_Entites_EntiteId",
                table: "Organigramme",
                column: "EntiteId",
                principalTable: "Entites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organigramme_TypePostes_TypePosteId",
                table: "Organigramme",
                column: "TypePosteId",
                principalTable: "TypePostes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
