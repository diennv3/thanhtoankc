using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Added_ChiTietThanhToan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietThanhToans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    SoTienThanhToan = table.Column<decimal>(nullable: false),
                    LoaiThanhToan = table.Column<int>(nullable: false),
                    NgayThanhToan = table.Column<DateTime>(nullable: false),
                    LichHenKhamId = table.Column<int>(nullable: true),
                    NguoiBenhId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietThanhToans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietThanhToans_LichHenKhams_LichHenKhamId",
                        column: x => x.LichHenKhamId,
                        principalTable: "LichHenKhams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietThanhToans_NguoiBenhs_NguoiBenhId",
                        column: x => x.NguoiBenhId,
                        principalTable: "NguoiBenhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToans_LichHenKhamId",
                table: "ChiTietThanhToans",
                column: "LichHenKhamId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToans_NguoiBenhId",
                table: "ChiTietThanhToans",
                column: "NguoiBenhId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToans_TenantId",
                table: "ChiTietThanhToans",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietThanhToans");
        }
    }
}
