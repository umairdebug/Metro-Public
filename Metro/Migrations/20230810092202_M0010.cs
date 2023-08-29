using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metro.Migrations
{
    /// <inheritdoc />
    public partial class M0010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_AppRole_AppRoleId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_AppRoleId",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "AppRoleId",
                table: "AppUser");

            migrationBuilder.CreateTable(
                name: "AppRoleAppUser",
                columns: table => new
                {
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleAppUser", x => new { x.RolesId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AppRoleAppUser_AppRole_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AppRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppRoleAppUser_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleAppUser_UserId",
                table: "AppRoleAppUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoleAppUser");

            migrationBuilder.AddColumn<string>(
                name: "AppRoleId",
                table: "AppUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_AppRoleId",
                table: "AppUser",
                column: "AppRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_AppRole_AppRoleId",
                table: "AppUser",
                column: "AppRoleId",
                principalTable: "AppRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
