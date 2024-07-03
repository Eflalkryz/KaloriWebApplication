using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaloriWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class Inıtıal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Eposta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminRole = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "CustomersProfiles",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    ActivityLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Goal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DailyCaloryLimit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersProfiles", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_CustomersProfiles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nutrients",
                columns: table => new
                {
                    NutrientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NutrientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NutrientEntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NutrientAmount = table.Column<int>(type: "int", nullable: false),
                    NutrientCalory = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    CustomersProfileCustomerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrients", x => x.NutrientID);
                    table.ForeignKey(
                        name: "FK_Nutrients_CustomersProfiles_CustomersProfileCustomerID",
                        column: x => x.CustomersProfileCustomerID,
                        principalTable: "CustomersProfiles",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "Calories",
                columns: table => new
                {
                    CaloryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    TotalCalory = table.Column<int>(type: "int", nullable: false),
                    CaloryEntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomersProfilesCustomerID = table.Column<int>(type: "int", nullable: true),
                    NutrientsNutrientID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calories", x => x.CaloryID);
                    table.ForeignKey(
                        name: "FK_Calories_CustomersProfiles_CustomersProfilesCustomerID",
                        column: x => x.CustomersProfilesCustomerID,
                        principalTable: "CustomersProfiles",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Calories_Nutrients_NutrientsNutrientID",
                        column: x => x.NutrientsNutrientID,
                        principalTable: "Nutrients",
                        principalColumn: "NutrientID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calories_CustomersProfilesCustomerID",
                table: "Calories",
                column: "CustomersProfilesCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Calories_NutrientsNutrientID",
                table: "Calories",
                column: "NutrientsNutrientID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersProfiles_UserID",
                table: "CustomersProfiles",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_CustomersProfileCustomerID",
                table: "Nutrients",
                column: "CustomersProfileCustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calories");

            migrationBuilder.DropTable(
                name: "Nutrients");

            migrationBuilder.DropTable(
                name: "CustomersProfiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
