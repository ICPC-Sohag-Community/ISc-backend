using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class StuffArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                schema: "ICPC",
                table: "StuffArchives",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<byte>(
                name: "College",
                schema: "ICPC",
                table: "NewRegisterations",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives",
                columns: new[] { "NationalId", "Role" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.AlterColumn<string>(
                name: "College",
                schema: "ICPC",
                table: "NewRegisterations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives",
                column: "NationalId");
        }
    }
}
