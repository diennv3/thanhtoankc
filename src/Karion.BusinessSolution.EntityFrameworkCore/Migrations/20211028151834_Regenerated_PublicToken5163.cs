using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_PublicToken5163 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NguoiBenhId",
                table: "PublicTokens",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublicTokens_NguoiBenhId",
                table: "PublicTokens",
                column: "NguoiBenhId");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicTokens_NguoiBenhs_NguoiBenhId",
                table: "PublicTokens",
                column: "NguoiBenhId",
                principalTable: "NguoiBenhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicTokens_NguoiBenhs_NguoiBenhId",
                table: "PublicTokens");

            migrationBuilder.DropIndex(
                name: "IX_PublicTokens_NguoiBenhId",
                table: "PublicTokens");

            migrationBuilder.DropColumn(
                name: "NguoiBenhId",
                table: "PublicTokens");
        }
    }
}
