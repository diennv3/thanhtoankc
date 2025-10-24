using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karion.BusinessSolution.Migrations
{
    public partial class Regenerated_NguoiBenh1697 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NguoiBenhs",
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
                    HoVaTen = table.Column<string>(maxLength: 250, nullable: false),
                    Tuoi = table.Column<string>(nullable: true),
                    GioiTinh = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(maxLength: 20, nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 15, nullable: false),
                    EmailAddress = table.Column<string>(maxLength: 250, nullable: false),
                    EmailConfirmationCode = table.Column<string>(maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    IsPhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    PasswordResetCode = table.Column<string>(maxLength: 500, nullable: true),
                    ProfilePicture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiBenhs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NguoiThans",
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
                    HoVaTen = table.Column<string>(maxLength: 250, nullable: false),
                    Tuoi = table.Column<string>(nullable: true),
                    GioiTinh = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    MoiQuanHe = table.Column<string>(maxLength: 20, nullable: true),
                    SoDienThoai = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiThans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NguoiBenhs_TenantId",
                table: "NguoiBenhs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThans_TenantId",
                table: "NguoiThans",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NguoiBenhs");

            migrationBuilder.DropTable(
                name: "NguoiThans");
        }
    }
}
