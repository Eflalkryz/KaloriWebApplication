using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaloriWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class WaterIntake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notificationTitle",
                table: "notifications",
                newName: "notificationText");

            migrationBuilder.RenameColumn(
                name: "notificationEntryDate",
                table: "notifications",
                newName: "notificationDate");

            migrationBuilder.AddColumn<int>(
                name: "isRead",
                table: "notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WaterIntakes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WaterAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateConsumed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterIntakes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterIntakes");

            migrationBuilder.DropColumn(
                name: "isRead",
                table: "notifications");

            migrationBuilder.RenameColumn(
                name: "notificationText",
                table: "notifications",
                newName: "notificationTitle");

            migrationBuilder.RenameColumn(
                name: "notificationDate",
                table: "notifications",
                newName: "notificationEntryDate");
        }
    }
}
