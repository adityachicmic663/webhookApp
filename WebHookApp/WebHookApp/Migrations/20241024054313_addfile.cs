using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebHookApp.Migrations
{
    /// <inheritdoc />
    public partial class addfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "filePath",
                table: "requests",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "filePath",
                table: "requests");
        }
    }
}
