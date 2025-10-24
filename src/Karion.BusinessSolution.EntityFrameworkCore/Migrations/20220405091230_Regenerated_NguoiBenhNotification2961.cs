using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiBenhNotification2961 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianGui",
                table: "NguoiBenhNotifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TieuDe",
                table: "NguoiBenhNotifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThoiGianGui",
                table: "NguoiBenhNotifications");

            migrationBuilder.DropColumn(
                name: "TieuDe",
                table: "NguoiBenhNotifications");
        }
    }
}
