using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiK_Test.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventParticipant",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParticipantsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipant", x => new { x.EventsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_EventParticipant_EventsTable_EventsId",
                        column: x => x.EventsId,
                        principalTable: "EventsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipant_ParticipantsTable_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "ParticipantsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_ParticipantsId",
                table: "EventParticipant",
                column: "ParticipantsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventParticipant");
        }
    }
}
