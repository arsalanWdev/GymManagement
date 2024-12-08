using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Migrations
{
    /// <inheritdoc />
    public partial class WalkInAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Trainers_TrainerId",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "Members",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "WalkIns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkIns", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccountOpenDate", "PackageExpiryDate" },
                values: new object[] { new DateTime(2024, 10, 8, 23, 45, 16, 975, DateTimeKind.Local).AddTicks(6851), new DateTime(2025, 1, 8, 23, 45, 16, 975, DateTimeKind.Local).AddTicks(6868) });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccountOpenDate", "PackageExpiryDate" },
                values: new object[] { new DateTime(2024, 11, 8, 23, 45, 16, 975, DateTimeKind.Local).AddTicks(7016), new DateTime(2024, 12, 23, 23, 45, 16, 975, DateTimeKind.Local).AddTicks(7017) });

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Trainers_TrainerId",
                table: "Members",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Trainers_TrainerId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "WalkIns");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Trainers_TrainerId",
                table: "Members",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
