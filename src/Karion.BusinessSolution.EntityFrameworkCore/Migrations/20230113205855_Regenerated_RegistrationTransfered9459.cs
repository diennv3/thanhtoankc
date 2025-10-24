using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_RegistrationTransfered9459 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NguoiBenhId",
                table: "RegistrationTransfereds",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "RegistrationTransfereds",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTransfereds_NguoiBenhId",
                table: "RegistrationTransfereds",
                column: "NguoiBenhId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationTransfereds_NguoiBenhs_NguoiBenhId",
                table: "RegistrationTransfereds",
                column: "NguoiBenhId",
                principalTable: "NguoiBenhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationTransfereds_NguoiBenhs_NguoiBenhId",
                table: "RegistrationTransfereds");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationTransfereds_NguoiBenhId",
                table: "RegistrationTransfereds");

            migrationBuilder.DropColumn(
                name: "NguoiBenhId",
                table: "RegistrationTransfereds");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "RegistrationTransfereds");
        }
    }
}
