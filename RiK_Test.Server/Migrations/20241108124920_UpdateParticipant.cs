using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiK_Test.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ParticipantsTable",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ParticipantsTable",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ParticipantsTable",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParticipantCount",
                table: "ParticipantsTable",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "ParticipantsTable",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ParticipantsTable");

            migrationBuilder.DropColumn(
                name: "ParticipantCount",
                table: "ParticipantsTable");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "ParticipantsTable");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ParticipantsTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "ParticipantsTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
