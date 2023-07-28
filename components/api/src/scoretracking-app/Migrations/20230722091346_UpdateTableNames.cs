using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreTracking.App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_game_games_game_id",
                table: "user_game");

            migrationBuilder.DropForeignKey(
                name: "fk_user_game_users_user_id",
                table: "user_game");

            migrationBuilder.DropForeignKey(
                name: "fk_user_game_round_rounds_round_id1",
                table: "user_game_round");

            migrationBuilder.DropForeignKey(
                name: "fk_user_game_round_user_game_user_game_id1",
                table: "user_game_round");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_game_round",
                table: "user_game_round");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_game",
                table: "user_game");

            migrationBuilder.RenameTable(
                name: "user_game_round",
                newName: "user_game_rounds");

            migrationBuilder.RenameTable(
                name: "user_game",
                newName: "user_games");

            migrationBuilder.RenameIndex(
                name: "ix_user_game_round_user_game_id",
                table: "user_game_rounds",
                newName: "ix_user_game_rounds_user_game_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_game_round_round_id",
                table: "user_game_rounds",
                newName: "ix_user_game_rounds_round_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_game_user_id",
                table: "user_games",
                newName: "ix_user_games_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_game_game_id",
                table: "user_games",
                newName: "ix_user_games_game_id");

            migrationBuilder.AddColumn<int>(
                name: "jokers",
                table: "user_game_rounds",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_game_rounds",
                table: "user_game_rounds",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_games",
                table: "user_games",
                column: "id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_user_game_rounds_rounds_round_id1",
                table: "user_game_rounds",
                column: "round_id",
                principalTable: "rounds",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_game_rounds_user_games_user_game_id1",
                table: "user_game_rounds",
                column: "user_game_id",
                principalTable: "user_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_games_games_game_id",
                table: "user_games",
                column: "game_id",
                principalTable: "games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_games_users_user_id",
                table: "user_games",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_game_rounds_rounds_round_id1",
                table: "user_game_rounds");

            migrationBuilder.DropForeignKey(
                name: "fk_user_game_rounds_user_games_user_game_id1",
                table: "user_game_rounds");

            migrationBuilder.DropForeignKey(
                name: "fk_user_games_games_game_id",
                table: "user_games");

            migrationBuilder.DropForeignKey(
                name: "fk_user_games_users_user_id",
                table: "user_games");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_games",
                table: "user_games");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_game_rounds",
                table: "user_game_rounds");

            migrationBuilder.DropColumn(
                name: "jokers",
                table: "user_game_rounds");

            migrationBuilder.RenameTable(
                name: "user_games",
                newName: "user_game");

            migrationBuilder.RenameTable(
                name: "user_game_rounds",
                newName: "user_game_round");

            migrationBuilder.RenameIndex(
                name: "ix_user_games_user_id",
                table: "user_game",
                newName: "ix_user_game_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_games_game_id",
                table: "user_game",
                newName: "ix_user_game_game_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_game_rounds_user_game_id",
                table: "user_game_round",
                newName: "ix_user_game_round_user_game_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_game_rounds_round_id",
                table: "user_game_round",
                newName: "ix_user_game_round_round_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_game",
                table: "user_game",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_game_round",
                table: "user_game_round",
                column: "id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_user_game_games_game_id",
                table: "user_game",
                column: "game_id",
                principalTable: "games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_game_users_user_id",
                table: "user_game",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_game_round_rounds_round_id1",
                table: "user_game_round",
                column: "round_id",
                principalTable: "rounds",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_game_round_user_game_user_game_id1",
                table: "user_game_round",
                column: "user_game_id",
                principalTable: "user_game",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
