using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Them_truong_bang_NguoiBenh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HanetStatus",
                table: "NguoiBenhs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "HanetStatus",
                table: "NguoiBenhs");
        }
    }
}
