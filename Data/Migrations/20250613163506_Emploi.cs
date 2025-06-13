using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Emploi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Emplois_EstActif",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_TypeContrat",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_UserId",
                table: "Emplois");

            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "Emplois");

            migrationBuilder.DropColumn(
                name: "Salaire",
                table: "Emplois");

            migrationBuilder.DropColumn(
                name: "SalaireDevise",
                table: "Emplois");

            migrationBuilder.DropColumn(
                name: "TypeContrat",
                table: "Emplois");

            migrationBuilder.AlterColumn<bool>(
                name: "EstActif",
                table: "Emplois",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "EstActif",
                table: "Emplois",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "Emplois",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Salaire",
                table: "Emplois",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalaireDevise",
                table: "Emplois",
                type: "TEXT",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeContrat",
                table: "Emplois",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_EstActif",
                table: "Emplois",
                column: "EstActif");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_TypeContrat",
                table: "Emplois",
                column: "TypeContrat");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_UserId",
                table: "Emplois",
                column: "UserId");
        }
    }
}
