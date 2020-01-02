using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BiuroPodróży.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jedzenie",
                columns: table => new
                {
                    Id_jedzenia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nazwa = table.Column<string>(maxLength: 50, nullable: false),
                    Opis = table.Column<string>(nullable: false),
                    Cena = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jedzenie", x => x.Id_jedzenia);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id_usera = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Imie = table.Column<string>(nullable: true),
                    Nazwisko = table.Column<string>(nullable: true),
                    Miejscowosc = table.Column<string>(nullable: true),
                    Telefon = table.Column<string>(nullable: true),
                    Uprawnienia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id_usera);
                });

            migrationBuilder.CreateTable(
                name: "Zakwaterowanie",
                columns: table => new
                {
                    Id_zakwaterowania = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nazwa = table.Column<string>(maxLength: 50, nullable: false),
                    Opis = table.Column<string>(nullable: false),
                    Cena = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zakwaterowanie", x => x.Id_zakwaterowania);
                });

            migrationBuilder.CreateTable(
                name: "Wycieczka",
                columns: table => new
                {
                    Id_wycieczki = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Miejsce = table.Column<string>(maxLength: 100, nullable: false),
                    Data_start = table.Column<DateTime>(nullable: false),
                    Data_end = table.Column<DateTime>(nullable: false),
                    Cena = table.Column<decimal>(nullable: false),
                    Opis = table.Column<string>(nullable: false),
                    Id_jedzenia = table.Column<int>(nullable: false),
                    Id_zakwaterowania = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wycieczka", x => x.Id_wycieczki);
                    table.ForeignKey(
                        name: "FK_Wycieczka_Jedzenie_Id_jedzenia",
                        column: x => x.Id_jedzenia,
                        principalTable: "Jedzenie",
                        principalColumn: "Id_jedzenia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wycieczka_Zakwaterowanie_Id_zakwaterowania",
                        column: x => x.Id_zakwaterowania,
                        principalTable: "Zakwaterowanie",
                        principalColumn: "Id_zakwaterowania",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wycieczka_Klient",
                columns: table => new
                {
                    Id_zamowienia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id_usera = table.Column<int>(nullable: false),
                    Id_wycieczki = table.Column<int>(nullable: false),
                    Bilety = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wycieczka_Klient", x => x.Id_zamowienia);
                    table.ForeignKey(
                        name: "FK_Wycieczka_Klient_User_Id_usera",
                        column: x => x.Id_usera,
                        principalTable: "User",
                        principalColumn: "Id_usera",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wycieczka_Klient_Wycieczka_Id_wycieczki",
                        column: x => x.Id_wycieczki,
                        principalTable: "Wycieczka",
                        principalColumn: "Id_wycieczki",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Login",
                table: "User",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wycieczka_Id_jedzenia",
                table: "Wycieczka",
                column: "Id_jedzenia");

            migrationBuilder.CreateIndex(
                name: "IX_Wycieczka_Id_zakwaterowania",
                table: "Wycieczka",
                column: "Id_zakwaterowania");

            migrationBuilder.CreateIndex(
                name: "IX_Wycieczka_Klient_Id_usera",
                table: "Wycieczka_Klient",
                column: "Id_usera");

            migrationBuilder.CreateIndex(
                name: "IX_Wycieczka_Klient_Id_wycieczki",
                table: "Wycieczka_Klient",
                column: "Id_wycieczki");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wycieczka_Klient");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Wycieczka");

            migrationBuilder.DropTable(
                name: "Jedzenie");

            migrationBuilder.DropTable(
                name: "Zakwaterowanie");
        }
    }
}
