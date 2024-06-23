using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddContest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "ICPC",
                table: "Sheets");

            migrationBuilder.CreateTable(
                name: "Contests",
                schema: "ICPC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Community = table.Column<byte>(type: "tinyint", nullable: false),
                    ProblemCount = table.Column<int>(type: "int", nullable: false),
                    CodeForceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CampId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contests_Camps_CampId",
                        column: x => x.CampId,
                        principalSchema: "ICPC",
                        principalTable: "Camps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contests_CampId",
                schema: "ICPC",
                table: "Contests",
                column: "CampId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contests",
                schema: "ICPC");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "ICPC",
                table: "Sheets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
