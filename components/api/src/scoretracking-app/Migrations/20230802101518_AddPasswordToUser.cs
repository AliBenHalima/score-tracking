using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreTracking.App.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "updated",
                table: "user_games",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "user_games",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "updated",
                table: "user_game_rounds",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "user_game_rounds",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "updated",
                table: "rounds",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "rounds",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "updated",
                table: "games",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "games",
                newName: "created_at");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "password_changed_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "verified_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password", "password_changed_at", "updated_at", "verified_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 8, 2, 10, 15, 18, 741, DateTimeKind.Unspecified).AddTicks(3519), new TimeSpan(0, 0, 0, 0, 0)), null, null, new DateTimeOffset(new DateTime(2023, 8, 2, 10, 15, 18, 741, DateTimeKind.Unspecified).AddTicks(3529), new TimeSpan(0, 0, 0, 0, 0)), null });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password", "password_changed_at", "updated_at", "verified_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 8, 2, 10, 15, 18, 741, DateTimeKind.Unspecified).AddTicks(3534), new TimeSpan(0, 0, 0, 0, 0)), null, null, new DateTimeOffset(new DateTime(2023, 8, 2, 10, 15, 18, 741, DateTimeKind.Unspecified).AddTicks(3535), new TimeSpan(0, 0, 0, 0, 0)), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_changed_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "verified_at",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "users",
                newName: "updated");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "users",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "user_games",
                newName: "updated");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "user_games",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "user_game_rounds",
                newName: "updated");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "user_game_rounds",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "rounds",
                newName: "updated");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "rounds",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "games",
                newName: "updated");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "games",
                newName: "created");

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
    }
}
