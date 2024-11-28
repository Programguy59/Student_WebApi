namespace Entities.Models
{
    public class DatabaseCommandStrings
    {
        public const string SqlString = @"SELECT s.StudentID, s.StudentName, s.StudentAddress, s.StudentNumberOfCourses, cl.ClassName, co.CourseName
                                            FROM  dbo.Class AS cl INNER JOIN
                                            dbo.StudentClass_RepetitionOnClass AS scc ON cl.ClassID = scc.ClassID INNER JOIN
                                            dbo.Student AS s ON s.StudentID = scc.StudentID INNER JOIN
                                            dbo.Course AS co ON scc.CourseID = co.CourseID
                                            WHERE(cl.ClassName LIKE '%')";

        public const string SQLString_SP = @"EXEC Student_Team_Course_Collected_SP";
    }
}
