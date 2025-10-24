using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiBenh7747 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "NguoiBenhs",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenExpire",
                table: "NguoiBenhs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "TokenExpire",
                table: "NguoiBenhs");
        }
    }
}
