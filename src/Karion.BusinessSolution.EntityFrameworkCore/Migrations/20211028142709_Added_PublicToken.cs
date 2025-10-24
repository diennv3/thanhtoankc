using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Added_PublicToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublicTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    TimeSet = table.Column<DateTime>(nullable: false),
                    TimeExpire = table.Column<DateTime>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    PrivateKey = table.Column<string>(nullable: true),
                    DeviceVerificationCode = table.Column<string>(maxLength: 500, nullable: true),
                    LastAccessDeviceVerificationCode = table.Column<string>(maxLength: 500, nullable: true),
                    IsTokenLocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicTokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicTokens_TenantId",
                table: "PublicTokens",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicTokens");
        }
    }
}
