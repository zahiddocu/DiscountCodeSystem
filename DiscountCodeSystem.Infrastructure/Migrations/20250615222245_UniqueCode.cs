using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscountCodeSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_Code",
                table: "DiscountCodes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_Code",
                table: "DiscountCodes");
        }
    }
}
