using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiBenh2964 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tuoi",
                table: "NguoiBenhs");

            migrationBuilder.AddColumn<string>(
                name: "NamSinh",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NgaySinh",
                table: "NguoiBenhs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ThangSinh",
                table: "NguoiBenhs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NamSinh",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "ThangSinh",
                table: "NguoiBenhs");

            migrationBuilder.AddColumn<string>(
                name: "Tuoi",
                table: "NguoiBenhs",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
