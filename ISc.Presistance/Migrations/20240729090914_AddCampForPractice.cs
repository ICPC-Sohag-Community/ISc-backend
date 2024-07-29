using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddCampForPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampId",
                schema: "ICPC",
                table: "Practice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Practice_CampId",
                schema: "ICPC",
                table: "Practice",
                column: "CampId");

            migrationBuilder.AddForeignKey(
                name: "FK_Practice_Camps_CampId",
                schema: "ICPC",
                table: "Practice",
                column: "CampId",
                principalSchema: "ICPC",
                principalTable: "Camps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practice_Camps_CampId",
                schema: "ICPC",
                table: "Practice");

            migrationBuilder.DropIndex(
                name: "IX_Practice_CampId",
                schema: "ICPC",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "CampId",
                schema: "ICPC",
                table: "Practice");
        }
    }
}
