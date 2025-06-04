using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailOtpToAlumniUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailOtp",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailOtpExpiration",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOtp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailOtpExpiration",
                table: "AspNetUsers");
        }
    }
}
