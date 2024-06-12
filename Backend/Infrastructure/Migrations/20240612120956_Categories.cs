using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ParticipantId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "EventsBase",
                newName: "CategoryId");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ParticipantId",
                table: "Tickets",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_EventsBase_CategoryId",
                table: "EventsBase",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventsBase_Categories_CategoryId",
                table: "EventsBase",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventsBase_Categories_CategoryId",
                table: "EventsBase");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ParticipantId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_EventsBase_CategoryId",
                table: "EventsBase");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "EventsBase",
                newName: "EventType");

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
        }
    }
}
