using Microsoft.EntityFrameworkCore.Migrations;

namespace BiuroPodróży.Migrations
{
    public partial class migrancja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NrTel",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NrTel",
                table: "AspNetUsers");
        }
    }
}
