using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeromylosSuites.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPrices_DateFrom",
                table: "SeasonalPrices",
                column: "DateFrom");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonalPrices_DateTo",
                table: "SeasonalPrices",
                column: "DateTo");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PhoneNumber",
                table: "Members",
                column: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeasonalPrices_DateFrom",
                table: "SeasonalPrices");

            migrationBuilder.DropIndex(
                name: "IX_SeasonalPrices_DateTo",
                table: "SeasonalPrices");

            migrationBuilder.DropIndex(
                name: "IX_Members_PhoneNumber",
                table: "Members");
        }
    }
}
