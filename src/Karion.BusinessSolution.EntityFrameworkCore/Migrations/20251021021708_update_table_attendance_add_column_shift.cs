using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class update_table_attendance_add_column_shift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "Attendances",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckOut",
                table: "Attendances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHalfDay",
                table: "Attendances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Shift",
                table: "Attendances",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsCheckOut",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsHalfDay",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Shift",
                table: "Attendances");
        }
    }
}
