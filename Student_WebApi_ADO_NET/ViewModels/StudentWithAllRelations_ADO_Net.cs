using Entities.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Student_WebApi_ADO_NET.ViewModels
{
    public class StudentWithAllRelations_ADO_Net : Student
    {
        public string ?Courses { get; set; }
        public string ?CourseIDs { get; set; }

        static StudentWithAllRelations_ADO_Net()
        {
            Int(TABLE_NAME, "Courses", (orm) => (orm as StudentWithAllRelations_ADO_Net).Courses);
            Int(TABLE_NAME, "CourseIDs", (orm) => (orm as StudentWithAllRelations_ADO_Net).CourseIDs);
        }

        public StudentWithAllRelations_ADO_Net()
        {

        }
    }
}
