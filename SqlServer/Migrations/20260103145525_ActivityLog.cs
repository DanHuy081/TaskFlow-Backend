using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsResetPasswordUsed",
            //    table: "Users",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "ResetPasswordExpire",
            //    table: "Users",
            //    type: "datetime2",
            //    nullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "ResetPasswordToken",
            //    table: "Users",
            //    type: "uniqueidentifier",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Notifications",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IsRead = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Notifications", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "User",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_User", x => x.UserId);
            //    });

            migrationBuilder.CreateTable(
                name: "activityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_activityLogs_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activityLogs_UserId",
                table: "activityLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activityLogs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "IsResetPasswordUsed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPasswordExpire",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Users");
        }
    }
}
