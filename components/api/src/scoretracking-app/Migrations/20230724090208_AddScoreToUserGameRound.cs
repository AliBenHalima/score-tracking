using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreTracking.App.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreToUserGameRound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "score",
                table: "user_game_rounds",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 24, 9, 2, 8, 71, DateTimeKind.Unspecified).AddTicks(7425), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 24, 9, 2, 8, 71, DateTimeKind.Unspecified).AddTicks(7435), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 24, 9, 2, 8, 71, DateTimeKind.Unspecified).AddTicks(7445), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 24, 9, 2, 8, 71, DateTimeKind.Unspecified).AddTicks(7445), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "score",
                table: "user_game_rounds");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 22, 9, 13, 46, 298, DateTimeKind.Unspecified).AddTicks(2858), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 22, 9, 13, 46, 298, DateTimeKind.Unspecified).AddTicks(2879), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 22, 9, 13, 46, 298, DateTimeKind.Unspecified).AddTicks(2887), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 22, 9, 13, 46, 298, DateTimeKind.Unspecified).AddTicks(2889), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
