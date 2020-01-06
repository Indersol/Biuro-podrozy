using Microsoft.EntityFrameworkCore.Migrations;

namespace BiuroPodróży.Migrations
{
    public partial class ExtendIdentityUserAU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Wycieczka_Klient",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wycieczka_Klient_Id",
                table: "Wycieczka_Klient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wycieczka_Klient_AspNetUsers_Id",
                table: "Wycieczka_Klient",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wycieczka_Klient_AspNetUsers_Id",
                table: "Wycieczka_Klient");

            migrationBuilder.DropIndex(
                name: "IX_Wycieczka_Klient_Id",
                table: "Wycieczka_Klient");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Wycieczka_Klient");
        }
    }
}
