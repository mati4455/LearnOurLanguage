using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model.Migrations
{
    public partial class DictionariesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DictionaryId",
                table: "GameSessions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Dictionaries",
                nullable: false,
                defaultValue: false);

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
                name: "IX_GameSessions_DictionaryId",
                table: "GameSessions",
                column: "DictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDictionaries_DictionaryId",
                table: "UserDictionaries",
                column: "DictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDictionaries_UserId",
                table: "UserDictionaries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Dictionaries_DictionaryId",
                table: "GameSessions",
                column: "DictionaryId",
                principalTable: "Dictionaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Dictionaries_DictionaryId",
                table: "GameSessions");

            migrationBuilder.DropTable(
                name: "UserDictionaries");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_DictionaryId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "DictionaryId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Public",
                table: "Dictionaries");
        }
    }
}
