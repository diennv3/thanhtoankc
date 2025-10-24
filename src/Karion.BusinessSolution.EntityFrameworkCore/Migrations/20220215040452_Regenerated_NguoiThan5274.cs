using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiThan5274 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NguoiThans_TenantId",
                table: "NguoiThans");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "NguoiThans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "NguoiThans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThans_TenantId",
                table: "NguoiThans",
                column: "TenantId");
        }
    }
}
