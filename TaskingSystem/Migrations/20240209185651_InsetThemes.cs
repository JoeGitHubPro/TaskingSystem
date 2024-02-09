using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskingSystem.Migrations
{
    /// <inheritdoc />
    public partial class InsetThemes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Themes",
               columns: new[] { "ThemeName", "ThemeSelected" },
               values: new object[,]
               {
                { "bootstrap.min", true },
                { "bootstrap.min (1)", false },
                { "bootstrap.min (2)", false },
                { "bootstrap.min (3)", false },
                { "bootstrap.min (4)", false },
                { "bootstrap.min (5)", false },
                { "bootstrap.min (6)", false },
                { "bootstrap.min (7)", false },
                { "bootstrap.min (8)", false },
                { "bootstrap.min (9)", false },
                { "bootstrap.min (10)",false },
                { "bootstrap.min (11)",false },
                { "bootstrap.min (12)",false },
                { "bootstrap.min (13)",false },
                { "bootstrap.min (14)",false },
                { "bootstrap.min (15)",false },
                { "bootstrap.min (16)",false },
                { "bootstrap.min (17)",false },
                { "bootstrap.min (18)",false },
                { "bootstrap.min (19)",false },
                { "bootstrap.min (20)",false },
                { "bootstrap.min (21)",false },
                { "bootstrap.min (22)",false },
                { "bootstrap.min (23)",false },
                { "bootstrap.min (24)",false },
                { "bootstrap.min (25)",false },
               });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
        table: "Themes",
        keyColumn: "Id",
        keyValues: null);
        }
    }
}
