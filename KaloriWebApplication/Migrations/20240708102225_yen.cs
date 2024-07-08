using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaloriWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class yen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_CustomersProfiles_CustomersProfileCustomerID",
                table: "Nutrients");

            migrationBuilder.DropIndex(
                name: "IX_Nutrients_CustomersProfileCustomerID",
                table: "Nutrients");

            migrationBuilder.DropColumn(
                name: "CustomersProfileCustomerID",
                table: "Nutrients");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Nutrients",
                newName: "UserID");

            migrationBuilder.CreateTable(
                name: "CaloryNutrients",
                columns: table => new
                {
                    NutrientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodItem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FoodCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    per100grams = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cals_per100grams = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KJ_per100grams = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaloryNutrients", x => x.NutrientID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_UserID",
                table: "Nutrients",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_Users_UserID",
                table: "Nutrients",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_Users_UserID",
                table: "Nutrients");

            migrationBuilder.DropTable(
                name: "CaloryNutrients");

            migrationBuilder.DropIndex(
                name: "IX_Nutrients_UserID",
                table: "Nutrients");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Nutrients",
                newName: "CustomerID");

            migrationBuilder.AddColumn<int>(
                name: "CustomersProfileCustomerID",
                table: "Nutrients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_CustomersProfileCustomerID",
                table: "Nutrients",
                column: "CustomersProfileCustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_CustomersProfiles_CustomersProfileCustomerID",
                table: "Nutrients",
                column: "CustomersProfileCustomerID",
                principalTable: "CustomersProfiles",
                principalColumn: "CustomerID");
        }
    }
}
