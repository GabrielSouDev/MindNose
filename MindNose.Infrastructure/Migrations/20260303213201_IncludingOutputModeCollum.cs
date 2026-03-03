using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindNose.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncludingOutputModeCollum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutputMode",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutputMode",
                table: "Messages");
        }
    }
}
