using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeromylosSuites.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceFromRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Rooms",
                type: "decimal(10,2)",
                nullable: true);
        }
    }
}
