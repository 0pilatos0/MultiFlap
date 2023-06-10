using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAchievement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Achievements");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "yes");

            migrationBuilder.UpdateData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "no");

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8568));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8663));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8670));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8676));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8696));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8703));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8711));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateAchieved",
                value: new DateTime(2023, 6, 10, 16, 6, 18, 12, DateTimeKind.Local).AddTicks(8716));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Achievements");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Achievements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsCompleted",
                value: true);

            migrationBuilder.UpdateData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsCompleted",
                value: false);

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
        }
    }
}
