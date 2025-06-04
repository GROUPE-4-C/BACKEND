using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlumniConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class AddResetPasswordOtpToAlumniUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordOtp",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordOtpExpiration",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordOtp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordOtpExpiration",
                table: "AspNetUsers");
        }
    }
}
