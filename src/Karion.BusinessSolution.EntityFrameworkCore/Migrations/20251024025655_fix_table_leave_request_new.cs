using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class fix_table_leave_request_new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveType",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "LeaveRequests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ApprovedByUserId",
                table: "LeaveRequests",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DurationHours",
                table: "LeaveRequests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HalfDayPart",
                table: "LeaveRequests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LeaveRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "DurationHours",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "HalfDayPart",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LeaveRequests");

            migrationBuilder.AddColumn<string>(
                name: "LeaveType",
                table: "LeaveRequests",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
