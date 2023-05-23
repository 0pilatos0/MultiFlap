using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class moveduserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SoundEnabled",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "UserSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SoundEnabled",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6687));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6741));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6743));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6745));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6747));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6749));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6751));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6753));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6755));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 23, 42, 3, 610, DateTimeKind.Local).AddTicks(6757));

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DisplayName", "SoundEnabled" },
                values: new object[] { "Admin1", false });

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DisplayName", "SoundEnabled" },
                values: new object[] { "Admin2", true });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "SoundEnabled",
                table: "UserSettings");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SoundEnabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(887));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(932));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(934));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(936));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(938));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(940));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(942));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(944));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(946));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(948));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DisplayName", "SoundEnabled" },
                values: new object[] { "Admin1", true });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DisplayName", "SoundEnabled" },
                values: new object[] { "Admin2", false });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId");
        }
    }
}
