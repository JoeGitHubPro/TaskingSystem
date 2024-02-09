using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskingSystem.Migrations
{
    /// <inheritdoc />
    public partial class MultipleThemesSelected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ThemeSelected",
                table: "Themes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeSelected",
                table: "Themes");
        }
    }
}
