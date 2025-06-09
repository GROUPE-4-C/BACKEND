using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionTableAndRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Promotion",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PromotionId",
                table: "AspNetUsers",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Promotions_PromotionId",
                table: "AspNetUsers",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Promotions_PromotionId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PromotionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Promotion",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
