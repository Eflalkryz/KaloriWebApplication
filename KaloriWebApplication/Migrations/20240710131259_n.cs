using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaloriWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class n : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calories_CustomersProfiles_CustomersProfilesCustomerID",
                table: "Calories");

            migrationBuilder.DropForeignKey(
                name: "FK_Calories_Nutrients_NutrientsNutrientID",
                table: "Calories");

            migrationBuilder.DropTable(
                name: "Nutrients");

            migrationBuilder.DropIndex(
                name: "IX_CustomersProfiles_UserID",
                table: "CustomersProfiles");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Calories");

            migrationBuilder.RenameColumn(
                name: "NutrientsNutrientID",
                table: "Calories",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CustomersProfilesCustomerID",
                table: "Calories",
                newName: "CustomersProfileCustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Calories_NutrientsNutrientID",
                table: "Calories",
                newName: "IX_Calories_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Calories_CustomersProfilesCustomerID",
                table: "Calories",
                newName: "IX_Calories_CustomersProfileCustomerID");

            migrationBuilder.AlterColumn<int>(
                name: "TotalCalory",
                table: "Calories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CaloryEntryDate",
                table: "Calories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "UserNutrients",
                columns: table => new
                {
                    UserNutrientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    NutrientID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateLogged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNutrients", x => x.UserNutrientID);
                    table.ForeignKey(
                        name: "FK_UserNutrients_CaloryNutrients_NutrientID",
                        column: x => x.NutrientID,
                        principalTable: "CaloryNutrients",
                        principalColumn: "NutrientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomersProfiles_UserID",
                table: "CustomersProfiles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserNutrients_NutrientID",
                table: "UserNutrients",
                column: "NutrientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_CustomersProfiles_CustomersProfileCustomerID",
                table: "Calories",
                column: "CustomersProfileCustomerID",
                principalTable: "CustomersProfiles",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_Users_UserID",
                table: "Calories",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calories_CustomersProfiles_CustomersProfileCustomerID",
                table: "Calories");

            migrationBuilder.DropForeignKey(
                name: "FK_Calories_Users_UserID",
                table: "Calories");

            migrationBuilder.DropTable(
                name: "UserNutrients");

            migrationBuilder.DropIndex(
                name: "IX_CustomersProfiles_UserID",
                table: "CustomersProfiles");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Calories",
                newName: "NutrientsNutrientID");

            migrationBuilder.RenameColumn(
                name: "CustomersProfileCustomerID",
                table: "Calories",
                newName: "CustomersProfilesCustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Calories_UserID",
                table: "Calories",
                newName: "IX_Calories_NutrientsNutrientID");

            migrationBuilder.RenameIndex(
                name: "IX_Calories_CustomersProfileCustomerID",
                table: "Calories",
                newName: "IX_Calories_CustomersProfilesCustomerID");

            migrationBuilder.AlterColumn<int>(
                name: "TotalCalory",
                table: "Calories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CaloryEntryDate",
                table: "Calories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Calories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Nutrients",
                columns: table => new
                {
                    NutrientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    NutrientAmount = table.Column<int>(type: "int", nullable: false),
                    NutrientCalory = table.Column<int>(type: "int", nullable: false),
                    NutrientEntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NutrientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrients", x => x.NutrientID);
                    table.ForeignKey(
                        name: "FK_Nutrients_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomersProfiles_UserID",
                table: "CustomersProfiles",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_UserID",
                table: "Nutrients",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_CustomersProfiles_CustomersProfilesCustomerID",
                table: "Calories",
                column: "CustomersProfilesCustomerID",
                principalTable: "CustomersProfiles",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_Nutrients_NutrientsNutrientID",
                table: "Calories",
                column: "NutrientsNutrientID",
                principalTable: "Nutrients",
                principalColumn: "NutrientID");
        }
    }
}
