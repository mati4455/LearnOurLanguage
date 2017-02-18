using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class GameDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "GameSessions",
                newName: "DateStart");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "GameSessionTranslations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                table: "GameSessions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "GameSessionTranslations");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "GameSessions");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "GameSessions",
                newName: "Date");
        }
    }
}
