using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaloriWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class secondmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calories_CustomersProfile_CustomersProfilesCustomerID",
                table: "Calories");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomersProfile_Users_UserID",
                table: "CustomersProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_CustomersProfile_CustomersProfileCustomerID",
                table: "Nutrients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomersProfile",
                table: "CustomersProfile");

            migrationBuilder.RenameTable(
                name: "CustomersProfile",
                newName: "CustomersProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_CustomersProfile_UserID",
                table: "CustomersProfiles",
                newName: "IX_CustomersProfiles_UserID");

            migrationBuilder.AlterColumn<int>(
                name: "CustomersProfileCustomerID",
                table: "Nutrients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersProfiles",
                table: "CustomersProfiles",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_CustomersProfiles_CustomersProfilesCustomerID",
                table: "Calories",
                column: "CustomersProfilesCustomerID",
                principalTable: "CustomersProfiles",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersProfiles_Users_UserID",
                table: "CustomersProfiles",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_CustomersProfiles_CustomersProfileCustomerID",
                table: "Nutrients",
                column: "CustomersProfileCustomerID",
                principalTable: "CustomersProfiles",
                principalColumn: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calories_CustomersProfiles_CustomersProfilesCustomerID",
                table: "Calories");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomersProfiles_Users_UserID",
                table: "CustomersProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_CustomersProfiles_CustomersProfileCustomerID",
                table: "Nutrients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomersProfiles",
                table: "CustomersProfiles");

            migrationBuilder.RenameTable(
                name: "CustomersProfiles",
                newName: "CustomersProfile");

            migrationBuilder.RenameIndex(
                name: "IX_CustomersProfiles_UserID",
                table: "CustomersProfile",
                newName: "IX_CustomersProfile_UserID");

            migrationBuilder.AlterColumn<int>(
                name: "CustomersProfileCustomerID",
                table: "Nutrients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomersProfile",
                table: "CustomersProfile",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calories_CustomersProfile_CustomersProfilesCustomerID",
                table: "Calories",
                column: "CustomersProfilesCustomerID",
                principalTable: "CustomersProfile",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersProfile_Users_UserID",
                table: "CustomersProfile",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_CustomersProfile_CustomersProfileCustomerID",
                table: "Nutrients",
                column: "CustomersProfileCustomerID",
                principalTable: "CustomersProfile",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
