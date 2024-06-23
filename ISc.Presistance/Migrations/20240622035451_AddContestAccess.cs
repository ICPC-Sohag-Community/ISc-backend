using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class AddContestAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TraineesContest",
                schema: "ICPC",
                columns: table => new
                {
                    TraineeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContestId = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineesContest", x => new { x.TraineeId, x.ContestId, x.Index });
                    table.ForeignKey(
                        name: "FK_TraineesContest_Contests_ContestId",
                        column: x => x.ContestId,
                        principalSchema: "ICPC",
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TraineesContest_Trainees_TraineeId",
                        column: x => x.TraineeId,
                        principalSchema: "ICPC",
                        principalTable: "Trainees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TraineesContest_ContestId",
                schema: "ICPC",
                table: "TraineesContest",
                column: "ContestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraineesContest",
                schema: "ICPC");
        }
    }
}
