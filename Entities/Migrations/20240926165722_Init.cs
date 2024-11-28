using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Core_8_0_Courses",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_8_0_Courses", x => x.CourseID);
                });

            migrationBuilder.CreateTable(
                name: "Core_8_0_Teams",
                columns: table => new
                {
                    TeamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_8_0_Teams", x => x.TeamID);
                });

            migrationBuilder.CreateTable(
                name: "Core_8_0_Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StudentLastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TeamID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_8_0_Students", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Core_8_0_Students_Core_8_0_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Core_8_0_Teams",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Core_8_0_StudentCourses",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_8_0_StudentCourses", x => new { x.StudentID, x.CourseID });
                    table.ForeignKey(
                        name: "FK_Core_8_0_StudentCourses_Core_8_0_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Core_8_0_Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Core_8_0_StudentCourses_Core_8_0_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Core_8_0_Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Core_8_0_StudentCourses_CourseID",
                table: "Core_8_0_StudentCourses",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Core_8_0_Students_TeamID",
                table: "Core_8_0_Students",
                column: "TeamID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Core_8_0_StudentCourses");

            migrationBuilder.DropTable(
                name: "Core_8_0_Courses");

            migrationBuilder.DropTable(
                name: "Core_8_0_Students");

            migrationBuilder.DropTable(
                name: "Core_8_0_Teams");
        }
    }
}
