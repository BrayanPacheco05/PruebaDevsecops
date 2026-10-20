using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace prueba.Migrations
{
    /// <inheritdoc />
    public partial class Cambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Contacto",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Telefonos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Numero = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactosTelefonos",
                columns: table => new
                {
                    ContactoId = table.Column<int>(type: "integer", nullable: false),
                    TelefonoId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactosTelefonos", x => new { x.ContactoId, x.TelefonoId });
                    table.ForeignKey(
                        name: "FK_ContactosTelefonos_Contacto_ContactoId",
                        column: x => x.ContactoId,
                        principalTable: "Contacto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactosTelefonos_Telefonos_TelefonoId",
                        column: x => x.TelefonoId,
                        principalTable: "Telefonos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacto_Email",
                table: "Contacto",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactosTelefonos_TelefonoId",
                table: "ContactosTelefonos",
                column: "TelefonoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactosTelefonos");

            migrationBuilder.DropTable(
                name: "Telefonos");

            migrationBuilder.DropIndex(
                name: "IX_Contacto_Email",
                table: "Contacto");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Contacto");
        }
    }
}
