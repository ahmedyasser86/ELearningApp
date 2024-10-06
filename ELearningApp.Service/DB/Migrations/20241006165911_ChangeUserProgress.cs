using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearningApp.Service.DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_AspNetUsers_ApplicationUserId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_ApplicationUserId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserProgresses");

            migrationBuilder.AddColumn<int>(
                name: "UserCourseId",
                table: "UserProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_UserCourseId",
                table: "UserProgresses",
                column: "UserCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_UserCourses_UserCourseId",
                table: "UserProgresses",
                column: "UserCourseId",
                principalTable: "UserCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_UserCourses_UserCourseId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_UserCourseId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "UserCourseId",
                table: "UserProgresses");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserProgresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_ApplicationUserId",
                table: "UserProgresses",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_AspNetUsers_ApplicationUserId",
                table: "UserProgresses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
