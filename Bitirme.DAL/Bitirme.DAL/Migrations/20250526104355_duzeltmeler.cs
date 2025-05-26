using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bitirme.DAL.Migrations
{
    /// <inheritdoc />
    public partial class duzeltmeler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ListeningSentence",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LessonStudents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListeningSentence",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LessonStudents");
        }
    }
}
