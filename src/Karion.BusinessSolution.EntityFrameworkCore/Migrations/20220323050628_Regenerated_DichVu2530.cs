using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_DichVu2530 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChuyenKhoaId",
                table: "DichVus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_ChuyenKhoaId",
                table: "DichVus",
                column: "ChuyenKhoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DichVus_ChuyenKhoas_ChuyenKhoaId",
                table: "DichVus",
                column: "ChuyenKhoaId",
                principalTable: "ChuyenKhoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DichVus_ChuyenKhoas_ChuyenKhoaId",
                table: "DichVus");

            migrationBuilder.DropIndex(
                name: "IX_DichVus_ChuyenKhoaId",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "ChuyenKhoaId",
                table: "DichVus");
        }
    }
}
