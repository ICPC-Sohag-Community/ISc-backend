using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class completeAccountAndRegisteration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "ICPC",
                table: "NewRegisterations",
                newName: "PhotoUrl");

            migrationBuilder.RenameColumn(
                name: "FacebookHandle",
                schema: "ICPC",
                table: "NewRegisterations",
                newName: "FacebookLink");

            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                schema: "Account",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookLink",
                schema: "Account",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                schema: "ICPC",
                table: "NewRegisterations",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "FacebookLink",
                schema: "ICPC",
                table: "NewRegisterations",
                newName: "FacebookHandle");
        }
    }
}
