using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class fix_table_leave_request_add_column_time_disom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllowedEarlyMinutes",
                table: "LeaveRequests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AllowedLateMinutes",
                table: "LeaveRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedEarlyMinutes",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "AllowedLateMinutes",
                table: "LeaveRequests");
        }
    }
}
