using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddShakeSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShakeEnabled",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5387));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5433));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5435));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5437));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5439));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5442));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5444));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5446));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5448));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 1, 23, 51, 58, 560, DateTimeKind.Local).AddTicks(5450));

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShakeEnabled",
                value: false);

            migrationBuilder.UpdateData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShakeEnabled",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShakeEnabled",
                table: "UserSettings");

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
        }
    }
}
