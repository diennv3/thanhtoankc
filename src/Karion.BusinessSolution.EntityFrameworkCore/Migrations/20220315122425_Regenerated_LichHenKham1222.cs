using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_LichHenKham1222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LichHenKhams_DichVus_DichVuId",
                table: "LichHenKhams");

            migrationBuilder.DropIndex(
                name: "IX_LichHenKhams_DichVuId",
                table: "LichHenKhams");

            migrationBuilder.DropColumn(
                name: "DichVuId",
                table: "LichHenKhams");

            migrationBuilder.AddColumn<string>(
                name: "ChiDinhDichVuSerialize",
                table: "LichHenKhams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Flag",
                table: "LichHenKhams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChiDinhDichVuSerialize",
                table: "LichHenKhams");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "LichHenKhams");

            migrationBuilder.AddColumn<int>(
                name: "DichVuId",
                table: "LichHenKhams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_DichVuId",
                table: "LichHenKhams",
                column: "DichVuId");

            migrationBuilder.AddForeignKey(
                name: "FK_LichHenKhams_DichVus_DichVuId",
                table: "LichHenKhams",
                column: "DichVuId",
                principalTable: "DichVus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
