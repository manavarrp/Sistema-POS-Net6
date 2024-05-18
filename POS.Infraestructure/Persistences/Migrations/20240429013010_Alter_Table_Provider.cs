using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infraestructure.Persistences.Migrations
{
    public partial class Alter_Table_Provider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK__Purcharse__Provi__7A672E12",
                table: "Purcharses");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProviderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Purcharses_Providers_ProviderId",
                table: "Purcharses",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purcharses_Providers_ProviderId",
                table: "Purcharses");

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProviderId",
                table: "Products",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK__Purcharse__Provi__7A672E12",
                table: "Purcharses",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "ProviderId");
        }
    }
}
