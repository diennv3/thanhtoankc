using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiThan4766 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThans_NguoiBenhs_NguoiBenhId",
                table: "NguoiThans");

            migrationBuilder.AlterColumn<int>(
                name: "NguoiBenhId",
                table: "NguoiThans",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiThans_NguoiBenhs_NguoiBenhId",
                table: "NguoiThans",
                column: "NguoiBenhId",
                principalTable: "NguoiBenhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThans_NguoiBenhs_NguoiBenhId",
                table: "NguoiThans");

            migrationBuilder.AlterColumn<int>(
                name: "NguoiBenhId",
                table: "NguoiThans",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiThans_NguoiBenhs_NguoiBenhId",
                table: "NguoiThans",
                column: "NguoiBenhId",
                principalTable: "NguoiBenhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
