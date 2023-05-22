using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class seeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "UserSettings");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DisplayName", "Email", "SoundEnabled" },
                values: new object[,]
                {
                    { 1, "Admin1", "admin1@example.com", true },
                    { 2, "Admin2", "admin2@example.com", false }
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "Id", "IsCompleted", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, true, "High Scorer", 1 },
                    { 2, false, "Speed Runner", 2 }
                });

            migrationBuilder.InsertData(
                table: "LeaderboardEntries",
                columns: new[] { "Id", "DateAchieved", "Score", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9859), 100, 1 },
                    { 2, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9909), 200, 2 },
                    { 3, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9911), 150, 1 },
                    { 4, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9913), 180, 2 },
                    { 5, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9915), 220, 1 },
                    { 6, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9917), 300, 2 },
                    { 7, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9919), 250, 1 },
                    { 8, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9921), 190, 2 },
                    { 9, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9923), 280, 1 },
                    { 10, new DateTime(2023, 5, 22, 22, 23, 54, 288, DateTimeKind.Local).AddTicks(9925), 230, 2 }
                });

            migrationBuilder.InsertData(
                table: "PowerUpItems",
                columns: new[] { "Id", "Name", "Quantity", "UserId" },
                values: new object[,]
                {
                    { 1, "Extra Life", 5, 1 },
                    { 2, "Super Boost", 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "UserSettings",
                columns: new[] { "Id", "Language", "ReceiveNotifications", "UserId" },
                values: new object[,]
                {
                    { 1, "English", true, 1 },
                    { 2, "French", false, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Achievements",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LeaderboardEntries",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PowerUpItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PowerUpItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "UserSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
