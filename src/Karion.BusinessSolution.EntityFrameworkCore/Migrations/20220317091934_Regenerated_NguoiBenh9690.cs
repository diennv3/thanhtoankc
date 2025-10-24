using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiBenh9690 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "GiaTriSuDungTuNgay",
                table: "NguoiBenhs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MaDonViBHXH",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDkBanDau",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoTheBHYT",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiDiemDuNam",
                table: "NguoiBenhs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaTriSuDungTuNgay",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "MaDonViBHXH",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "NoiDkBanDau",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "SoTheBHYT",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "ThoiDiemDuNam",
                table: "NguoiBenhs");
        }
    }
}
