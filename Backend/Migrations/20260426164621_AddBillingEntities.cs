using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Factures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DateFacture = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalHT = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    TotalTVA = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    TotalTTC = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    TimbreFiscal = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Statut = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factures_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParametresFiscaux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimbreFiscal = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametresFiscaux", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LigneFactures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FactureId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProduitId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantite = table.Column<int>(type: "INTEGER", nullable: false),
                    PrixUnitaireHT = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    TauxTVA = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MontantHT = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    MontantTVA = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    MontantTTC = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LigneFactures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LigneFactures_Factures_FactureId",
                        column: x => x.FactureId,
                        principalTable: "Factures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LigneFactures_Produits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "Produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Factures_ClientId",
                table: "Factures",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_LigneFactures_FactureId",
                table: "LigneFactures",
                column: "FactureId");

            migrationBuilder.CreateIndex(
                name: "IX_LigneFactures_ProduitId",
                table: "LigneFactures",
                column: "ProduitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LigneFactures");

            migrationBuilder.DropTable(
                name: "ParametresFiscaux");

            migrationBuilder.DropTable(
                name: "Factures");
        }
    }
}
