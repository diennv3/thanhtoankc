using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Added_HanetTenantDeviceDatas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HanetTenantDeviceDatases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    deviceId = table.Column<string>(nullable: true),
                    deviceName = table.Column<string>(nullable: true),
                    deviceStatus = table.Column<bool>(nullable: false),
                    lastCheck = table.Column<DateTime>(nullable: false),
                    HanetTenantPlaceDatasId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HanetTenantDeviceDatases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HanetTenantDeviceDatases_HanetTenantPlaceDatases_HanetTenant~",
                        column: x => x.HanetTenantPlaceDatasId,
                        principalTable: "HanetTenantPlaceDatases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HanetTenantDeviceDatases_HanetTenantPlaceDatasId",
                table: "HanetTenantDeviceDatases",
                column: "HanetTenantPlaceDatasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HanetTenantDeviceDatases");
        }
    }
}
