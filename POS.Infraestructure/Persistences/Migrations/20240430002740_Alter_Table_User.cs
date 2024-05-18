using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infraestructure.Persistences.Migrations
{
    public partial class Alter_Table_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purcharses_Users_UserId",
                table: "Purcharses");

            migrationBuilder.DropIndex(
                name: "IX_Purcharses_UserId",
                table: "Purcharses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Purcharses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Purcharses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purcharses_UserId",
                table: "Purcharses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purcharses_Users_UserId",
                table: "Purcharses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
