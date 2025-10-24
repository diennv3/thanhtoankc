using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_Attendance8142 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CheckOutLongitude",
                table: "Attendances",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CheckOutLatitude",
                table: "Attendances",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CheckOutFaceMatchPercentage",
                table: "Attendances",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CheckInLongitude",
                table: "Attendances",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<double>(
                name: "CheckInLatitude",
                table: "Attendances",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<double>(
                name: "CheckInFaceMatchPercentage",
                table: "Attendances",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CheckOutLongitude",
                table: "Attendances",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CheckOutLatitude",
                table: "Attendances",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CheckOutFaceMatchPercentage",
                table: "Attendances",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CheckInLongitude",
                table: "Attendances",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "CheckInLatitude",
                table: "Attendances",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "CheckInFaceMatchPercentage",
                table: "Attendances",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
