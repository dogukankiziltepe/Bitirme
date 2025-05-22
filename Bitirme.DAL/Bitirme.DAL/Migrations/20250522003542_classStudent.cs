using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bitirme.DAL.Migrations
{
    /// <inheritdoc />
    public partial class classStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassStudent");

            migrationBuilder.DropIndex(
                name: "IX_Medias_LessonId",
                table: "Medias");

            migrationBuilder.AddColumn<int>(
                name: "ClassMediaId",
                table: "Lesson",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClassStudents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ClassId = table.Column<string>(type: "text", nullable: false),
                    StudentId = table.Column<string>(type: "text", nullable: true),
                    RecordStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassStudents_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QuestionString = table.Column<string>(type: "text", nullable: false),
                    AnswerOne = table.Column<string>(type: "text", nullable: true),
                    AnswerTwo = table.Column<string>(type: "text", nullable: true),
                    AnswerThree = table.Column<string>(type: "text", nullable: true),
                    AnswerFour = table.Column<string>(type: "text", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "text", nullable: false),
                    ClassId = table.Column<string>(type: "text", nullable: true),
                    LessonId = table.Column<string>(type: "text", nullable: true),
                    QuestionMediaId = table.Column<string>(type: "text", nullable: true),
                    CourseId = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Medias_QuestionMediaId",
                        column: x => x.QuestionMediaId,
                        principalTable: "Medias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_LessonId",
                table: "Medias",
                column: "LessonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudents_ClassId",
                table: "ClassStudents",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudents_StudentId",
                table: "ClassStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ClassId",
                table: "Questions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CourseId",
                table: "Questions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_LessonId",
                table: "Questions",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionMediaId",
                table: "Questions",
                column: "QuestionMediaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassStudents");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Medias_LessonId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "ClassMediaId",
                table: "Lesson");

            migrationBuilder.CreateTable(
                name: "ClassStudent",
                columns: table => new
                {
                    ClassesId = table.Column<string>(type: "text", nullable: false),
                    StudentsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudent", x => new { x.ClassesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_ClassStudent_Classes_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_LessonId",
                table: "Medias",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudent_StudentsId",
                table: "ClassStudent",
                column: "StudentsId");
        }
    }
}
