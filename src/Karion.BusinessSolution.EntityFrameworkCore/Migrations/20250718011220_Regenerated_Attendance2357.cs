using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_Attendance2357 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckOutDeviceInfo",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckOutFaceMatchPercentage",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckOutLatitude",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckOutLongitude",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsCheckOutFaceMatched",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsEarlyCheckOut",
                table: "Attendances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "Attendances",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckOutDeviceInfo",
                table: "Attendances",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckOutFaceMatchPercentage",
                table: "Attendances",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLatitude",
                table: "Attendances",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLongitude",
                table: "Attendances",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckOutFaceMatched",
                table: "Attendances",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEarlyCheckOut",
                table: "Attendances",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
