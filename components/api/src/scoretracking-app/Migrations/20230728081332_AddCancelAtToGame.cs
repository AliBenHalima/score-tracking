using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreTracking.App.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelAtToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "canceled_at",
                table: "games",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 28, 8, 13, 31, 920, DateTimeKind.Unspecified).AddTicks(6576), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 28, 8, 13, 31, 920, DateTimeKind.Unspecified).AddTicks(6586), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created", "updated" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 28, 8, 13, 31, 920, DateTimeKind.Unspecified).AddTicks(6589), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 28, 8, 13, 31, 920, DateTimeKind.Unspecified).AddTicks(6589), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "canceled_at",
                table: "games");

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
    }
}
