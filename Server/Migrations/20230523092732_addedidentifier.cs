using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class addedidentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auth0Identifier",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

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
                column: "Auth0Identifier",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Auth0Identifier",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth0Identifier",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9909));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9911));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9913));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9915));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9917));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9919));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9921));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9923));

            migrationBuilder.UpdateData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateAchieved",
                value: new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9925));
        }
    }
}
