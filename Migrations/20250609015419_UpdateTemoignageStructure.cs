using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTemoignageStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Temoignages",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Temoignages_UserId",
                table: "Temoignages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Temoignages_AspNetUsers_UserId",
                table: "Temoignages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Temoignages_AspNetUsers_UserId",
                table: "Temoignages");

            migrationBuilder.DropIndex(
                name: "IX_Temoignages_UserId",
                table: "Temoignages");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Temoignages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
