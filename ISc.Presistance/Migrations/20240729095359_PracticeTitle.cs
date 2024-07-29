using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class PracticeTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "ICPC",
                table: "Practice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                schema: "ICPC",
                table: "Practice");
        }
    }
}
