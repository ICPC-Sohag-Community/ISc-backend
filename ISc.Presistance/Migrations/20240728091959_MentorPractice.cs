using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class MentorPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadsOfCamps_Users_id",
                schema: "ICPC",
                table: "HeadsOfCamps");

            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Users_id",
                schema: "ICPC",
                table: "Mentors");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Users_id",
                schema: "ICPC",
                table: "Trainees");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "Account",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "Account",
                table: "UserClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "TraineeTasks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "TraineesArchives",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Trainees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "StuffArchives",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Sheets",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Sessions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "Account",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "Account",
                table: "RoleClaims",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Notifications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "NewRegisterations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Mentors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Materials",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "HeadsOfCamps",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Contests",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "ICPC",
                table: "Camps",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "VjudgeHandle",
                schema: "ICPC",
                table: "StuffArchives",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodeForceHandle",
                schema: "ICPC",
                table: "StuffArchives",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.CreateTable(
                name: "Practice",
                schema: "ICPC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetingLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<byte>(type: "tinyint", nullable: false),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practice_Mentors_MentorId",
                        column: x => x.MentorId,
                        principalSchema: "ICPC",
                        principalTable: "Mentors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Practice_MentorId",
                schema: "ICPC",
                table: "Practice",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfCamps_Users_Id",
                schema: "ICPC",
                table: "HeadsOfCamps",
                column: "Id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Users_Id",
                schema: "ICPC",
                table: "Mentors",
                column: "Id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Users_Id",
                schema: "ICPC",
                table: "Trainees",
                column: "Id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadsOfCamps_Users_Id",
                schema: "ICPC",
                table: "HeadsOfCamps");

            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Users_Id",
                schema: "ICPC",
                table: "Mentors");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Users_Id",
                schema: "ICPC",
                table: "Trainees");

            migrationBuilder.DropTable(
                name: "Practice",
                schema: "ICPC");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Account",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Account",
                table: "UserClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "TraineeTasks",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "TraineesArchives",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Trainees",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "StuffArchives",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Sheets",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Sessions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Account",
                table: "Roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Account",
                table: "RoleClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Notifications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "NewRegisterations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Mentors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Materials",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "HeadsOfCamps",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Contests",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "ICPC",
                table: "Camps",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "VjudgeHandle",
                schema: "ICPC",
                table: "StuffArchives",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodeForceHandle",
                schema: "ICPC",
                table: "StuffArchives",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfCamps_Users_id",
                schema: "ICPC",
                table: "HeadsOfCamps",
                column: "id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Users_id",
                schema: "ICPC",
                table: "Mentors",
                column: "id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Users_id",
                schema: "ICPC",
                table: "Trainees",
                column: "id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
