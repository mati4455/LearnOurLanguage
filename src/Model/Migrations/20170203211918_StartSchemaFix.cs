using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class StartSchemaFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamesSessions_Games_GameId",
                table: "GamesSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_GamesSessions_Users_UserId",
                table: "GamesSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessionTranslations_GamesSessions_GameSessionId",
                table: "GameSessionTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamesSessions",
                table: "GamesSessions");

            migrationBuilder.RenameTable(
                name: "GamesSessions",
                newName: "GameSessions");

            migrationBuilder.RenameIndex(
                name: "IX_GamesSessions_UserId",
                table: "GameSessions",
                newName: "IX_GameSessions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GamesSessions_GameId",
                table: "GameSessions",
                newName: "IX_GameSessions_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameSessions",
                table: "GameSessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Games_GameId",
                table: "GameSessions",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Users_UserId",
                table: "GameSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessionTranslations_GameSessions_GameSessionId",
                table: "GameSessionTranslations",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Games_GameId",
                table: "GameSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Users_UserId",
                table: "GameSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessionTranslations_GameSessions_GameSessionId",
                table: "GameSessionTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameSessions",
                table: "GameSessions");

            migrationBuilder.RenameTable(
                name: "GameSessions",
                newName: "GamesSessions");

            migrationBuilder.RenameIndex(
                name: "IX_GameSessions_UserId",
                table: "GamesSessions",
                newName: "IX_GamesSessions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GameSessions_GameId",
                table: "GamesSessions",
                newName: "IX_GamesSessions_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamesSessions",
                table: "GamesSessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GamesSessions_Games_GameId",
                table: "GamesSessions",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GamesSessions_Users_UserId",
                table: "GamesSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessionTranslations_GamesSessions_GameSessionId",
                table: "GameSessionTranslations",
                column: "GameSessionId",
                principalTable: "GamesSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
