using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TraineesArchives",
                schema: "ICPC",
                table: "TraineesArchives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "ICPC",
                table: "TraineesArchives",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "ICPC",
                table: "StuffArchives",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TraineesArchives",
                schema: "ICPC",
                table: "TraineesArchives",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TraineesArchives_NationalId_CampName",
                schema: "ICPC",
                table: "TraineesArchives",
                columns: new[] { "NationalId", "CampName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StuffArchives_NationalId_Role",
                schema: "ICPC",
                table: "StuffArchives",
                columns: new[] { "NationalId", "Role" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TraineesArchives",
                schema: "ICPC",
                table: "TraineesArchives");

            migrationBuilder.DropIndex(
                name: "IX_TraineesArchives_NationalId_CampName",
                schema: "ICPC",
                table: "TraineesArchives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.DropIndex(
                name: "IX_StuffArchives_NationalId_Role",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "ICPC",
                table: "TraineesArchives");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TraineesArchives",
                schema: "ICPC",
                table: "TraineesArchives",
                columns: new[] { "NationalId", "CampName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StuffArchives",
                schema: "ICPC",
                table: "StuffArchives",
                columns: new[] { "NationalId", "Role" });
        }
    }
}
