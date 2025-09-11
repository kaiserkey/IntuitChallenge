using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Intuit.Api.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Cuit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "ClientId", "Address", "BirthDate", "Cuit", "Email", "FirstName", "LastName", "Mobile" },
                values: new object[,]
                {
                    { 1, "Calle Falsa 123", new DateOnly(1990, 5, 12), "27-12345678-5", "ana@example.com", "Ana", "García", "+5491122334455" },
                    { 2, "Av. Siempreviva 742", new DateOnly(1985, 4, 8), "20-11111111-9", "luis@example.com", "Luis", "Pérez", "+5491144455566" },
                    { 3, "Belgrano 101", new DateOnly(1988, 7, 1), "20-22222222-3", "carlos@example.com", "Carlos", "López", "+5491100000003" },
                    { 4, "Mitre 220", new DateOnly(1992, 3, 22), "27-23456789-2", "maria@example.com", "María", "Fernández", "+5491100000004" },
                    { 5, "Rivadavia 330", new DateOnly(1991, 9, 14), "20-33333333-6", "julian@example.com", "Julián", "Martínez", "+5491100000005" },
                    { 6, "Sarmiento 450", new DateOnly(1993, 1, 5), "27-34567890-1", "sofia@example.com", "Sofía", "Rodríguez", "+5491100000006" },
                    { 7, "San Martín 512", new DateOnly(1987, 11, 2), "20-44444444-8", "pedro@example.com", "Pedro", "Sánchez", "+5491100000007" },
                    { 8, "Alsina 600", new DateOnly(1995, 6, 30), "27-45678901-0", "lucia@example.com", "Lucía", "Gómez", "+5491100000008" },
                    { 9, "Urquiza 710", new DateOnly(1989, 10, 18), "20-55555555-5", "diego@example.com", "Diego", "Torres", "+5491100000009" },
                    { 10, "Moreno 820", new DateOnly(1994, 2, 27), "27-56789012-8", "valentina@example.com", "Valentina", "Díaz", "+5491100000010" },
                    { 11, "Lavalle 930", new DateOnly(1986, 12, 9), "20-66666666-1", "matias@example.com", "Matías", "Romero", "+5491100000011" },
                    { 12, "Catamarca 1040", new DateOnly(1990, 8, 3), "27-67890123-6", "camila@example.com", "Camila", "Ruiz", "+5491100000012" },
                    { 13, "Córdoba 1150", new DateOnly(1983, 4, 16), "20-77777777-7", "fernando@example.com", "Fernando", "Gutiérrez", "+5491100000013" },
                    { 14, "Tucumán 1260", new DateOnly(1991, 1, 25), "27-78901234-4", "paula@example.com", "Paula", "Silva", "+5491100000014" },
                    { 15, "Salta 1370", new DateOnly(1988, 5, 7), "20-88888888-9", "agustin@example.com", "Agustín", "Molina", "+5491100000015" },
                    { 16, "Mendoza 1480", new DateOnly(1996, 7, 19), "27-80123456-7", "julieta@example.com", "Julieta", "Castro", "+5491100000016" },
                    { 17, "Entre Ríos 1590", new DateOnly(1984, 3, 11), "20-99999999-0", "ramiro@example.com", "Ramiro", "Navarro", "+5491100000017" },
                    { 18, "San Juan 1700", new DateOnly(1993, 9, 2), "27-81234567-5", "bianca@example.com", "Bianca", "Rojas", "+5491100000018" },
                    { 19, "Santiago 1810", new DateOnly(1987, 2, 13), "20-12121212-3", "nicolas@example.com", "Nicolás", "Vega", "+5491100000019" },
                    { 20, "Jujuy 1920", new DateOnly(1992, 11, 21), "27-82345678-3", "elena@example.com", "Elena", "Acosta", "+5491100000020" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_Cuit",
                table: "Client",
                column: "Cuit",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_Email",
                table: "Client",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
