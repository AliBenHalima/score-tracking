using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreTracking.App.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberToRound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "number",
                table: "rounds",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 21, 17, 51, 7, 779, DateTimeKind.Unspecified).AddTicks(1024), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 21, 17, 51, 7, 779, DateTimeKind.Unspecified).AddTicks(1037), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 21, 17, 51, 7, 779, DateTimeKind.Unspecified).AddTicks(1043), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 21, 17, 51, 7, 779, DateTimeKind.Unspecified).AddTicks(1044), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "number",
                table: "rounds");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 21, 9, 29, 59, 610, DateTimeKind.Unspecified).AddTicks(2192), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 21, 9, 29, 59, 610, DateTimeKind.Unspecified).AddTicks(2204), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 21, 9, 29, 59, 610, DateTimeKind.Unspecified).AddTicks(2208), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 21, 9, 29, 59, 610, DateTimeKind.Unspecified).AddTicks(2209), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
