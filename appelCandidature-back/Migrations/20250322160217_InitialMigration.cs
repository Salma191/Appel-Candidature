using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pfe_back.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypePostes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypePostes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilisateurs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Poste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exigence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatePublication = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroUnique = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Critere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypePosteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poste_TypePostes_TypePosteId",
                        column: x => x.TypePosteId,
                        principalTable: "TypePostes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntiteId = table.Column<int>(type: "int", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateValidation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypePosteId = table.Column<int>(type: "int", nullable: false),
                    CommissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PV_Commissions_CommissionId",
                        column: x => x.CommissionId,
                        principalTable: "Commissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PV_Entites_EntiteId",
                        column: x => x.EntiteId,
                        principalTable: "Entites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PV_TypePostes_TypePosteId",
                        column: x => x.TypePosteId,
                        principalTable: "TypePostes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AffectationActuelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRetraite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoursAbsence = table.Column<int>(type: "int", nullable: false),
                    Sanction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteTroisDernieresAnnees = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Catégorie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Congé = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosteOccupe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Consentement = table.Column<bool>(type: "bit", nullable: false),
                    UtilisateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidats_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembreCommissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleCommission = table.Column<int>(type: "int", nullable: false),
                    CommissionId = table.Column<int>(type: "int", nullable: false),
                    UtilisateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembreCommissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembreCommissions_Commissions_CommissionId",
                        column: x => x.CommissionId,
                        principalTable: "Commissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembreCommissions_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntiteId = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateSignature = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatePublication = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NbreMaxPosteCandidat = table.Column<int>(type: "int", nullable: false),
                    PVId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decision_Entites_EntiteId",
                        column: x => x.EntiteId,
                        principalTable: "Entites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decision_PV_PVId",
                        column: x => x.PVId,
                        principalTable: "PV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PVPoste",
                columns: table => new
                {
                    PVId = table.Column<int>(type: "int", nullable: false),
                    PosteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PVPoste", x => new { x.PVId, x.PosteId });
                    table.ForeignKey(
                        name: "FK_PVPoste_PV_PVId",
                        column: x => x.PVId,
                        principalTable: "PV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PVPoste_Poste_PosteId",
                        column: x => x.PosteId,
                        principalTable: "Poste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroEnregistrement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatePostulation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CandidatId = table.Column<int>(type: "int", nullable: false),
                    PosteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidatures_Candidats_CandidatId",
                        column: x => x.CandidatId,
                        principalTable: "Candidats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidatures_Poste_PosteId",
                        column: x => x.PosteId,
                        principalTable: "Poste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diplomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intitulé = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Etablissement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnéeObtention = table.Column<int>(type: "int", nullable: false),
                    Niveau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diplomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diplomes_Candidats_CandidatId",
                        column: x => x.CandidatId,
                        principalTable: "Candidats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Poste = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Entreprise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDébut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiences_Candidats_CandidatId",
                        column: x => x.CandidatId,
                        principalTable: "Candidats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecisionPoste",
                columns: table => new
                {
                    DecisionId = table.Column<int>(type: "int", nullable: false),
                    PosteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionPoste", x => new { x.DecisionId, x.PosteId });
                    table.ForeignKey(
                        name: "FK_DecisionPoste_Decision_DecisionId",
                        column: x => x.DecisionId,
                        principalTable: "Decision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DecisionPoste_Poste_PosteId",
                        column: x => x.PosteId,
                        principalTable: "Poste",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Phases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phases_Decision_DecisionId",
                        column: x => x.DecisionId,
                        principalTable: "Decision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PieceJointes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fichier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PVId = table.Column<int>(type: "int", nullable: true),
                    DecisionId = table.Column<int>(type: "int", nullable: true),
                    IsFromPV = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PieceJointes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PieceJointes_Decision_DecisionId",
                        column: x => x.DecisionId,
                        principalTable: "Decision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PieceJointes_PV_PVId",
                        column: x => x.PVId,
                        principalTable: "PV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Candidatures_CandidatureId",
                        column: x => x.CandidatureId,
                        principalTable: "Candidatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateEnvoi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CandidatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Candidatures_CandidatureId",
                        column: x => x.CandidatureId,
                        principalTable: "Candidatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { 1, "Administrateur" },
                    { 2, "DAO" },
                    { 3, "Candidat" },
                    { 4, "MembreCommission" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidats_UtilisateurId",
                table: "Candidats",
                column: "UtilisateurId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_CandidatId",
                table: "Candidatures",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_PosteId",
                table: "Candidatures",
                column: "PosteId");

            migrationBuilder.CreateIndex(
                name: "IX_Decision_EntiteId",
                table: "Decision",
                column: "EntiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Decision_PVId",
                table: "Decision",
                column: "PVId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DecisionPoste_PosteId",
                table: "DecisionPoste",
                column: "PosteId");

            migrationBuilder.CreateIndex(
                name: "IX_Diplomes_CandidatId",
                table: "Diplomes",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CandidatureId",
                table: "Documents",
                column: "CandidatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_CandidatId",
                table: "Experiences",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_MembreCommissions_CommissionId",
                table: "MembreCommissions",
                column: "CommissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MembreCommissions_UtilisateurId",
                table: "MembreCommissions",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_MembreCommissions_UtilisateurId_CommissionId",
                table: "MembreCommissions",
                columns: new[] { "UtilisateurId", "CommissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CandidatureId",
                table: "Notifications",
                column: "CandidatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Phases_DecisionId",
                table: "Phases",
                column: "DecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_PieceJointes_DecisionId",
                table: "PieceJointes",
                column: "DecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_PieceJointes_PVId",
                table: "PieceJointes",
                column: "PVId");

            migrationBuilder.CreateIndex(
                name: "IX_Poste_TypePosteId",
                table: "Poste",
                column: "TypePosteId");

            migrationBuilder.CreateIndex(
                name: "IX_PV_CommissionId",
                table: "PV",
                column: "CommissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PV_EntiteId",
                table: "PV",
                column: "EntiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PV_TypePosteId",
                table: "PV",
                column: "TypePosteId");

            migrationBuilder.CreateIndex(
                name: "IX_PVPoste_PosteId",
                table: "PVPoste",
                column: "PosteId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_RoleId",
                table: "Utilisateurs",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionPoste");

            migrationBuilder.DropTable(
                name: "Diplomes");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "MembreCommissions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Phases");

            migrationBuilder.DropTable(
                name: "PieceJointes");

            migrationBuilder.DropTable(
                name: "PVPoste");

            migrationBuilder.DropTable(
                name: "Candidatures");

            migrationBuilder.DropTable(
                name: "Decision");

            migrationBuilder.DropTable(
                name: "Candidats");

            migrationBuilder.DropTable(
                name: "Poste");

            migrationBuilder.DropTable(
                name: "PV");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "Entites");

            migrationBuilder.DropTable(
                name: "TypePostes");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
