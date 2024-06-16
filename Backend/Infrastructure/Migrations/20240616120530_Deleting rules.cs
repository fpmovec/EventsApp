using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Deletingrules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_EventsBase_EventId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_EventsBase_EventId",
                table: "Images",
                column: "EventId",
                principalTable: "EventsBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_EventsBase_EventId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_EventsBase_EventId",
                table: "Images",
                column: "EventId",
                principalTable: "EventsBase",
                principalColumn: "Id");
        }
    }
}
