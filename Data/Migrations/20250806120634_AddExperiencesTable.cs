using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExperiencesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Poste = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Entreprise = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Localisation = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EnCours = table.Column<bool>(type: "INTEGER", nullable: false),
                    TypeContrat = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Secteur = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateModification = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_DateDebut",
                table: "Experiences",
                column: "DateDebut");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_EnCours",
                table: "Experiences",
                column: "EnCours");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_Entreprise",
                table: "Experiences",
                column: "Entreprise");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_UserId",
                table: "Experiences",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiences");
        }
    }
}
