using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsToTeamMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CanEditTask",
                table: "TeamMembers",
                newName: "CanUploadFiles");

            migrationBuilder.RenameColumn(
                name: "CanDeleteTask",
                table: "TeamMembers",
                newName: "CanSetTaskDueDate");

            migrationBuilder.RenameColumn(
                name: "CanCreateTask",
                table: "TeamMembers",
                newName: "CanEditTasks");

            migrationBuilder.AddColumn<bool>(
                name: "CanAssignTasks",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanChangeTaskPriority",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanChangeTaskStatus",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanCommentOnTasks",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanCreateTasks",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteTasks",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanAssignTasks",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "CanChangeTaskPriority",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "CanChangeTaskStatus",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "CanCommentOnTasks",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "CanCreateTasks",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "CanDeleteTasks",
                table: "TeamMembers");

            migrationBuilder.RenameColumn(
                name: "CanUploadFiles",
                table: "TeamMembers",
                newName: "CanEditTask");

            migrationBuilder.RenameColumn(
                name: "CanSetTaskDueDate",
                table: "TeamMembers",
                newName: "CanDeleteTask");

            migrationBuilder.RenameColumn(
                name: "CanEditTasks",
                table: "TeamMembers",
                newName: "CanCreateTask");
        }
    }
}
