using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class update_table_nguoibenh_shift_attendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Shift",
                table: "Attendances");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "Shifts",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "Shifts",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<int>(
                name: "AssignedShiftId",
                table: "NguoiBenhs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Attendances",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShiftName",
                table: "Attendances",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiBenhs_AssignedShiftId",
                table: "NguoiBenhs",
                column: "AssignedShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ShiftId",
                table: "Attendances",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Shifts_ShiftId",
                table: "Attendances",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiBenhs_Shifts_AssignedShiftId",
                table: "NguoiBenhs",
                column: "AssignedShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Shifts_ShiftId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiBenhs_Shifts_AssignedShiftId",
                table: "NguoiBenhs");

            migrationBuilder.DropIndex(
                name: "IX_NguoiBenhs_AssignedShiftId",
                table: "NguoiBenhs");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ShiftId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "AssignedShiftId",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ShiftName",
                table: "Attendances");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Shifts",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Shifts",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<int>(
                name: "WorkDaysMask",
                table: "NguoiBenhs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkShiftEnd",
                table: "NguoiBenhs",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkShiftName",
                table: "NguoiBenhs",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkShiftStart",
                table: "NguoiBenhs",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shift",
                table: "Attendances",
                type: "varchar(128) CHARACTER SET utf8mb4",
                maxLength: 128,
                nullable: true);
        }
    }
}
