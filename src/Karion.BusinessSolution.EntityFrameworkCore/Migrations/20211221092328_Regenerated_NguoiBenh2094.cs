using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiBenh2094 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NguoiBenhs_TenantId",
                table: "NguoiBenhs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "NguoiBenhs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "NguoiBenhs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiBenhs_TenantId",
                table: "NguoiBenhs",
                column: "TenantId");
        }
    }
}
