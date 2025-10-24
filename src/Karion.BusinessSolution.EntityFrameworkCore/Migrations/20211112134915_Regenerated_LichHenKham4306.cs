using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_LichHenKham4306 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NguoiThanId",
                table: "LichHenKhams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NguoiBenhId",
                table: "LichHenKhams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DichVuId",
                table: "LichHenKhams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ChuyenKhoaId",
                table: "LichHenKhams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_ChuyenKhoaId",
                table: "LichHenKhams",
                column: "ChuyenKhoaId");

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_DichVuId",
                table: "LichHenKhams",
                column: "DichVuId");

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_NguoiBenhId",
                table: "LichHenKhams",
                column: "NguoiBenhId");

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_NguoiThanId",
                table: "LichHenKhams",
                column: "NguoiThanId");

            migrationBuilder.AddForeignKey(
                name: "FK_LichHenKhams_ChuyenKhoas_ChuyenKhoaId",
                table: "LichHenKhams",
                column: "ChuyenKhoaId",
                principalTable: "ChuyenKhoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichHenKhams_DichVus_DichVuId",
                table: "LichHenKhams",
                column: "DichVuId",
                principalTable: "DichVus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichHenKhams_NguoiBenhs_NguoiBenhId",
                table: "LichHenKhams",
                column: "NguoiBenhId",
                principalTable: "NguoiBenhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichHenKhams_NguoiThans_NguoiThanId",
                table: "LichHenKhams",
                column: "NguoiThanId",
                principalTable: "NguoiThans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LichHenKhams_ChuyenKhoas_ChuyenKhoaId",
                table: "LichHenKhams");

            migrationBuilder.DropForeignKey(
                name: "FK_LichHenKhams_DichVus_DichVuId",
                table: "LichHenKhams");

            migrationBuilder.DropForeignKey(
                name: "FK_LichHenKhams_NguoiBenhs_NguoiBenhId",
                table: "LichHenKhams");

            migrationBuilder.DropForeignKey(
                name: "FK_LichHenKhams_NguoiThans_NguoiThanId",
                table: "LichHenKhams");

            migrationBuilder.DropIndex(
                name: "IX_LichHenKhams_ChuyenKhoaId",
                table: "LichHenKhams");

            migrationBuilder.DropIndex(
                name: "IX_LichHenKhams_DichVuId",
                table: "LichHenKhams");

            migrationBuilder.DropIndex(
                name: "IX_LichHenKhams_NguoiBenhId",
                table: "LichHenKhams");

            migrationBuilder.DropIndex(
                name: "IX_LichHenKhams_NguoiThanId",
                table: "LichHenKhams");

            migrationBuilder.AlterColumn<int>(
                name: "NguoiThanId",
                table: "LichHenKhams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NguoiBenhId",
                table: "LichHenKhams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DichVuId",
                table: "LichHenKhams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChuyenKhoaId",
                table: "LichHenKhams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
