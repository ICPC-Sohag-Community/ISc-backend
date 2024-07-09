using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "ICPC",
                table: "Materials");

            migrationBuilder.AddColumn<int>(
                name: "MaterialOrder",
                schema: "ICPC",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "ICPC",
                table: "Materials",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialOrder",
                schema: "ICPC",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "ICPC",
                table: "Materials");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "ICPC",
                table: "Materials",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
