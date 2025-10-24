using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Added_LichHenKham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LichHenKhams",
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
                    NguoiBenhId = table.Column<int>(nullable: false),
                    NguoiThanId = table.Column<int>(nullable: false),
                    DichVuId = table.Column<int>(nullable: false),
                    ChuyenKhoaId = table.Column<int>(nullable: false),
                    NgayHenKham = table.Column<DateTime>(nullable: false),
                    MoTaTrieuChung = table.Column<string>(nullable: false),
                    IsCoBHYT = table.Column<bool>(nullable: false),
                    SoTheBHYT = table.Column<string>(maxLength: 50, nullable: false),
                    NoiDangKyKCBDauTien = table.Column<string>(maxLength: 250, nullable: false),
                    BHYTValidDate = table.Column<DateTime>(nullable: false),
                    PhuongThucThanhToan = table.Column<int>(nullable: false),
                    IsDaKham = table.Column<bool>(nullable: false),
                    IsDaThanhToan = table.Column<bool>(nullable: false),
                    TimeHoanThanhKham = table.Column<DateTime>(nullable: false),
                    TimeHoanThanhThanhToan = table.Column<DateTime>(nullable: false),
                    BacSiId = table.Column<long>(nullable: true),
                    ThuNganId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichHenKhams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichHenKhams_AbpUsers_BacSiId",
                        column: x => x.BacSiId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichHenKhams_AbpUsers_ThuNganId",
                        column: x => x.ThuNganId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_BacSiId",
                table: "LichHenKhams",
                column: "BacSiId");

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_TenantId",
                table: "LichHenKhams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LichHenKhams_ThuNganId",
                table: "LichHenKhams",
                column: "ThuNganId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichHenKhams");
        }
    }
}
