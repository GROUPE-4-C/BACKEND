using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmploiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Salaire",
                table: "Emplois",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EstActif",
                table: "Emplois",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_DateFin",
                table: "Emplois",
                column: "DateFin");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_EstActif",
                table: "Emplois",
                column: "EstActif");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_Localisation",
                table: "Emplois",
                column: "Localisation");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_TypeContrat",
                table: "Emplois",
                column: "TypeContrat");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_UserId",
                table: "Emplois",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Emplois_DateFin",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_EstActif",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_Localisation",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_TypeContrat",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_UserId",
                table: "Emplois");

            migrationBuilder.AlterColumn<decimal>(
                name: "Salaire",
                table: "Emplois",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EstActif",
                table: "Emplois",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: true);
        }
    }
}
