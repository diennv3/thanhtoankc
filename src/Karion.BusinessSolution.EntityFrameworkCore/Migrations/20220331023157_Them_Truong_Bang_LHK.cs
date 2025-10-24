using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Them_Truong_Bang_LHK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TienThua",
                table: "LichHenKhams",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienDaThanhToan",
                table: "LichHenKhams",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TienThua",
                table: "LichHenKhams");

            migrationBuilder.DropColumn(
                name: "TongTienDaThanhToan",
                table: "LichHenKhams");
        }
    }
}
