using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_Attendance3545 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOvertime",
                table: "Attendances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OvertimeEnd",
                table: "Attendances",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OvertimeStart",
                table: "Attendances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOvertime",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "OvertimeEnd",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "OvertimeStart",
                table: "Attendances");
        }
    }
}
