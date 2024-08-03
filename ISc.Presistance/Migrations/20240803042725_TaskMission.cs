using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class TaskMission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Task",
                schema: "ICPC",
                table: "TraineeTasks",
                newName: "Title");

            migrationBuilder.CreateTable(
                name: "TaskMissions",
                schema: "ICPC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TraineeTaskId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskMissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskMissions_TraineeTasks_TraineeTaskId",
                        column: x => x.TraineeTaskId,
                        principalSchema: "ICPC",
                        principalTable: "TraineeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskMissions_TraineeTaskId",
                schema: "ICPC",
                table: "TaskMissions",
                column: "TraineeTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskMissions",
                schema: "ICPC");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "ICPC",
                table: "TraineeTasks",
                newName: "Task");
        }
    }
}
