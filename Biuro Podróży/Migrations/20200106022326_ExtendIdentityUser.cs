using Microsoft.EntityFrameworkCore.Migrations;

namespace BiuroPodróży.Migrations
{
    public partial class ExtendIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imie",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Miasto",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nazwisko",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imie",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Miasto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nazwisko",
                table: "AspNetUsers");
        }
    }
}
