using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metro.Migrations
{
    /// <inheritdoc />
    public partial class M007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "AppUser");

            migrationBuilder.RenameColumn(
                name: "imageUrl",
                table: "ProductDisplaySection",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ProductDisplaySection",
                newName: "imageUrl");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "AppUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
