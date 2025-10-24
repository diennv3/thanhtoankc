using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Added_BacSiChuyenKhoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NguoiBenhTests");

            migrationBuilder.CreateTable(
                name: "BacSiChuyenKhoas",
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
                    UserId = table.Column<long>(nullable: false),
                    ChuyenKhoaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacSiChuyenKhoas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BacSiChuyenKhoas_ChuyenKhoas_ChuyenKhoaId",
                        column: x => x.ChuyenKhoaId,
                        principalTable: "ChuyenKhoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BacSiChuyenKhoas_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BacSiChuyenKhoas_ChuyenKhoaId",
                table: "BacSiChuyenKhoas",
                column: "ChuyenKhoaId");

            migrationBuilder.CreateIndex(
                name: "IX_BacSiChuyenKhoas_TenantId",
                table: "BacSiChuyenKhoas",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BacSiChuyenKhoas_UserId",
                table: "BacSiChuyenKhoas",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BacSiChuyenKhoas");

            migrationBuilder.CreateTable(
                name: "NguoiBenhTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Ten = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiBenhTests", x => x.Id);
                });
        }
    }
}
