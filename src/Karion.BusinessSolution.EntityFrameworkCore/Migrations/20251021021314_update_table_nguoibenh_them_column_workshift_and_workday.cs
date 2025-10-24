using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class update_table_nguoibenh_them_column_workshift_and_workday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkDaysMask",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkShiftEnd",
                table: "NguoiBenhs",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkShiftName",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkShiftStart",
                table: "NguoiBenhs",
                type: "time",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkDaysMask",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "WorkShiftEnd",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "WorkShiftName",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "WorkShiftStart",
                table: "NguoiBenhs");
        }
    }
}
