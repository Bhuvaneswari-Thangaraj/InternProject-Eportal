using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElearningPortal.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cname",
                table: "enrollDbSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollDate",
                table: "enrollDbSet",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Mentor",
                table: "enrollDbSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tname",
                table: "enrollDbSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cname",
                table: "enrollDbSet");

            migrationBuilder.DropColumn(
                name: "EnrollDate",
                table: "enrollDbSet");

            migrationBuilder.DropColumn(
                name: "Mentor",
                table: "enrollDbSet");

            migrationBuilder.DropColumn(
                name: "Tname",
                table: "enrollDbSet");
        }
    }
}
