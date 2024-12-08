using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Migrations
{
    /// <inheritdoc />
    public partial class keyadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccountOpenDate", "PackageExpiryDate" },
                values: new object[] { new DateTime(2024, 10, 8, 13, 48, 32, 745, DateTimeKind.Local).AddTicks(6073), new DateTime(2025, 1, 8, 13, 48, 32, 745, DateTimeKind.Local).AddTicks(6089) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountOpenDate", "PackageExpiryDate" },
                values: new object[] { new DateTime(2024, 11, 8, 13, 48, 32, 745, DateTimeKind.Local).AddTicks(6101), new DateTime(2024, 12, 23, 13, 48, 32, 745, DateTimeKind.Local).AddTicks(6101) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccountOpenDate", "PackageExpiryDate" },
                values: new object[] { new DateTime(2024, 10, 8, 13, 29, 27, 587, DateTimeKind.Local).AddTicks(6320), new DateTime(2025, 1, 8, 13, 29, 27, 587, DateTimeKind.Local).AddTicks(6340) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountOpenDate", "PackageExpiryDate" },
                values: new object[] { new DateTime(2024, 11, 8, 13, 29, 27, 587, DateTimeKind.Local).AddTicks(6352), new DateTime(2024, 12, 23, 13, 29, 27, 587, DateTimeKind.Local).AddTicks(6354) });
        }
    }
}
