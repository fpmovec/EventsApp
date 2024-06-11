using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TicketRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_EventsBase_EventId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ParticipantId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ParticipantId",
                table: "Tickets",
                column: "ParticipantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ExtendedEvents_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "ExtendedEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ExtendedEvents_EventId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ParticipantId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ParticipantId",
                table: "Tickets",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_EventsBase_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "EventsBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
