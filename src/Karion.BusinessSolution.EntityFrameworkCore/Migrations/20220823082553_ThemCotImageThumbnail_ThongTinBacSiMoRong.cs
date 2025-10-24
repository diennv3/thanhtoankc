using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class ThemCotImageThumbnail_ThongTinBacSiMoRong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageThumbnail",
                table: "ThongTinBacSiMoRongs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageThumbnail",
                table: "ThongTinBacSiMoRongs");
        }
    }
}
