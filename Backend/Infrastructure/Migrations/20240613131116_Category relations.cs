using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Categoryrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventsBase_CategoryId",
                table: "EventsBase");

            migrationBuilder.CreateIndex(
                name: "IX_EventsBase_CategoryId",
                table: "EventsBase",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventsBase_CategoryId",
                table: "EventsBase");

            migrationBuilder.CreateIndex(
                name: "IX_EventsBase_CategoryId",
                table: "EventsBase",
                column: "CategoryId",
                unique: true);
        }
    }
}
