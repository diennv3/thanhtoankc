using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Added_GiaDichVu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiaDichVus",
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
                    MucGia = table.Column<string>(maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(nullable: true),
                    Gia = table.Column<int>(nullable: false),
                    NgayApDung = table.Column<DateTime>(nullable: false),
                    DichVuId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaDichVus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiaDichVus_DichVus_DichVuId",
                        column: x => x.DichVuId,
                        principalTable: "DichVus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiaDichVus_DichVuId",
                table: "GiaDichVus",
                column: "DichVuId");

            migrationBuilder.CreateIndex(
                name: "IX_GiaDichVus_TenantId",
                table: "GiaDichVus",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiaDichVus");
        }
    }
}
