using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISc.Presistance.Migrations
{
    /// <inheritdoc />
    public partial class CreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "TraineeTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "TraineesArchives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "TraineeAttendences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "StuffArchives",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Sheets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "SessionFeedbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Practice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "NewRegisterations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "MentorsOfCamps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Contests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Camps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Account",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "TraineeTasks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "TraineesArchives");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "TraineeAttendences");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "StuffArchives");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Sheets");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "SessionFeedbacks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Practice");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "NewRegisterations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "MentorsOfCamps");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ICPC",
                table: "Camps");
        }
    }
}
