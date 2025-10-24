using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiThan9324 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Tuoi",
                table: "NguoiThans",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiBenhId",
                table: "NguoiThans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThans_NguoiBenhId",
                table: "NguoiThans",
                column: "NguoiBenhId");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiThans_NguoiBenhs_NguoiBenhId",
                table: "NguoiThans",
                column: "NguoiBenhId",
                principalTable: "NguoiBenhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThans_NguoiBenhs_NguoiBenhId",
                table: "NguoiThans");

            migrationBuilder.DropIndex(
                name: "IX_NguoiThans_NguoiBenhId",
                table: "NguoiThans");

            migrationBuilder.DropColumn(
                name: "NguoiBenhId",
                table: "NguoiThans");

            migrationBuilder.AlterColumn<string>(
                name: "Tuoi",
                table: "NguoiThans",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
