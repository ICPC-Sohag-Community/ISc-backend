using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMentorSessionAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Sessions_SessionId",
                schema: "ICPC",
                table: "Mentors");

            migrationBuilder.DropIndex(
                name: "IX_Mentors_SessionId",
                schema: "ICPC",
                table: "Mentors");

            migrationBuilder.DropColumn(
                name: "SessionId",
                schema: "ICPC",
                table: "Mentors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                schema: "ICPC",
                table: "Mentors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mentors_SessionId",
                schema: "ICPC",
                table: "Mentors",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Sessions_SessionId",
                schema: "ICPC",
                table: "Mentors",
                column: "SessionId",
                principalSchema: "ICPC",
                principalTable: "Sessions",
                principalColumn: "id");
        }
    }
}
