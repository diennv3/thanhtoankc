using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_BacSiDichVu5365 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BacSiDichVus_DichVus_DichVuId",
                table: "BacSiDichVus");

            migrationBuilder.DropForeignKey(
                name: "FK_BacSiDichVus_AbpUsers_UserId",
                table: "BacSiDichVus");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "BacSiDichVus",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DichVuId",
                table: "BacSiDichVus",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BacSiDichVus_DichVus_DichVuId",
                table: "BacSiDichVus",
                column: "DichVuId",
                principalTable: "DichVus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BacSiDichVus_AbpUsers_UserId",
                table: "BacSiDichVus",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BacSiDichVus_DichVus_DichVuId",
                table: "BacSiDichVus");

            migrationBuilder.DropForeignKey(
                name: "FK_BacSiDichVus_AbpUsers_UserId",
                table: "BacSiDichVus");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "BacSiDichVus",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "DichVuId",
                table: "BacSiDichVus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BacSiDichVus_DichVus_DichVuId",
                table: "BacSiDichVus",
                column: "DichVuId",
                principalTable: "DichVus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BacSiDichVus_AbpUsers_UserId",
                table: "BacSiDichVus",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
