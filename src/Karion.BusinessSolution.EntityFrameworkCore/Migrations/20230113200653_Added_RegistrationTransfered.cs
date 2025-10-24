using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Added_RegistrationTransfered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationTransfereds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    LichHenKhamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationTransfereds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationTransfereds_LichHenKhams_LichHenKhamId",
                        column: x => x.LichHenKhamId,
                        principalTable: "LichHenKhams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTransfereds_LichHenKhamId",
                table: "RegistrationTransfereds",
                column: "LichHenKhamId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTransfereds_TenantId",
                table: "RegistrationTransfereds",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationTransfereds");
        }
    }
}
