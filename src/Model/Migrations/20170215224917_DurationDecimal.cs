using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class DurationDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Duration",
                table: "GameSessionTranslations",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "GameSessionTranslations",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
