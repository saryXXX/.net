using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class PerformanceOptimizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Produits_Nom",
                table: "Produits",
                column: "Nom");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_DateFacture",
                table: "Factures",
                column: "DateFacture");

            migrationBuilder.CreateIndex(
                name: "IX_Factures_Numero",
                table: "Factures",
                column: "Numero",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Produits_Nom",
                table: "Produits");

            migrationBuilder.DropIndex(
                name: "IX_Factures_DateFacture",
                table: "Factures");

            migrationBuilder.DropIndex(
                name: "IX_Factures_Numero",
                table: "Factures");
        }
    }
}
