using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model.Migrations
{
    public partial class UpdateDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDictionaries");

            migrationBuilder.AddColumn<int>(
                name: "ParentDictionaryId",
                table: "Dictionaries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentDictionaryId",
                table: "Dictionaries");

            migrationBuilder.CreateTable(
                name: "UserDictionaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DictionaryId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDictionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDictionaries_Dictionaries_DictionaryId",
                        column: x => x.DictionaryId,
                        principalTable: "Dictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDictionaries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDictionaries_DictionaryId",
                table: "UserDictionaries",
                column: "DictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDictionaries_UserId",
                table: "UserDictionaries",
                column: "UserId");
        }
    }
}
