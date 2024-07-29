using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaloriWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class noty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    notificationEntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notificationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.notificationID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notifications");

           

            migrationBuilder.CreateIndex(
                name: "IX_Calory_CustomersProfileCustomerID",
                table: "Calory",
                column: "CustomersProfileCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Calory_UserID",
                table: "Calory",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersProfiles_UserID",
                table: "CustomersProfiles",
                column: "UserID");
        }
    }
}
