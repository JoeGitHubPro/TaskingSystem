using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class designAssignmentsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessorId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseCode);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentHeadLines",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfessorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentHeadLines", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_AssignmentHeadLines_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentHeadLines_Users_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsCourses",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsCourses", x => new { x.StudentId, x.CourseCode });
                    table.ForeignKey(
                        name: "FK_StudentsCourses_Courses_CourseCode",
                        column: x => x.CourseCode,
                        principalTable: "Courses",
                        principalColumn: "CourseCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsCourses_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignedTasks",
                columns: table => new
                {
                    AssignedTaskId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    TaskURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedTaskDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedTaskStudentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedTasks", x => x.AssignedTaskId);
                    table.ForeignKey(
                        name: "FK_AssignedTasks_AssignmentHeadLines_AssignedTaskId",
                        column: x => x.AssignedTaskId,
                        principalTable: "AssignmentHeadLines",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignedTasks_Users_AssignedTaskStudentId",
                        column: x => x.AssignedTaskStudentId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTasks_AssignedTaskStudentId",
                table: "AssignedTasks",
                column: "AssignedTaskStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHeadLines_CourseCode",
                table: "AssignmentHeadLines",
                column: "CourseCode");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentHeadLines_ProfessorId",
                table: "AssignmentHeadLines",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsCourses_CourseCode",
                table: "StudentsCourses",
                column: "CourseCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedTasks");

            migrationBuilder.DropTable(
                name: "StudentsCourses");

            migrationBuilder.DropTable(
                name: "AssignmentHeadLines");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
