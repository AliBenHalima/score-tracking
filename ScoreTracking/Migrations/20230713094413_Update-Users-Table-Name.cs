using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreTracking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsersTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEntities",
                table: "UserEntities");

            migrationBuilder.RenameTable(
                name: "UserEntities",
                newName: "users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "UserEntities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEntities",
                table: "UserEntities",
                column: "Id");
        }
    }
}
