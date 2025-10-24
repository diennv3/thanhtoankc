using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_Attendance4192 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceInfo",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "FaceMatchPercentage",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsFaceMatched",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Attendances");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOut",
                table: "Attendances",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "CheckInDeviceInfo",
                table: "Attendances",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CheckInFaceMatchPercentage",
                table: "Attendances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CheckInLatitude",
                table: "Attendances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CheckInLongitude",
                table: "Attendances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CheckOutDeviceInfo",
                table: "Attendances",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CheckOutFaceMatchPercentage",
                table: "Attendances",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CheckOutLatitude",
                table: "Attendances",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CheckOutLongitude",
                table: "Attendances",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckInFaceMatched",
                table: "Attendances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckOutFaceMatched",
                table: "Attendances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEarlyCheckOut",
                table: "Attendances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLateCheckIn",
                table: "Attendances",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInDeviceInfo",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckInFaceMatchPercentage",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckInLatitude",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckInLongitude",
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
                name: "IsCheckInFaceMatched",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsCheckOutFaceMatched",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsEarlyCheckOut",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsLateCheckIn",
                table: "Attendances");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOut",
                table: "Attendances",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceInfo",
                table: "Attendances",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FaceMatchPercentage",
                table: "Attendances",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFaceMatched",
                table: "Attendances",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Attendances",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Attendances",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
