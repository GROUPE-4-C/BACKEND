using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEmploiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emplois",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Entreprise = table.Column<string>(type: "TEXT", nullable: false),
                    Localisation = table.Column<string>(type: "TEXT", nullable: false),
                    TypeContrat = table.Column<string>(type: "TEXT", nullable: false),
                    Salaire = table.Column<decimal>(type: "TEXT", nullable: true),
                    SalaireDevise = table.Column<string>(type: "TEXT", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    EstActif = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emplois", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emplois");
        }
    }
}
