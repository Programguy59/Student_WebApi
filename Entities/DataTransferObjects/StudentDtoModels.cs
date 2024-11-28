using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class StudentForSaveDto
    {
        public string ?StudentName { get; set; }

        public string ?StudentLastName { get; set; }

        public virtual int TeamID { get; set; }
    }

    public class StudentForUpdateDto : StudentForSaveDto
    {
        public int StudentID { get; set; }
    }

    public class StudentDtoMinusRelations : StudentForUpdateDto
    {
        public override int TeamID { get; set; }

        public TeamDtoMinusRelations Team { get; set; }
    }

    public class StudentDto : StudentDtoMinusRelations
    {
        public ICollection<StudentCourseDtoMinusStudentRelations> StudentCourses { get; set; }
                  = new List<StudentCourseDtoMinusStudentRelations>();
    }

    public class StudentDtoMinusTeamRelations : StudentForUpdateDto
    {
        public ICollection<StudentCourseDtoMinusStudentRelations> StudentCourses { get; set; }
                  = new List<StudentCourseDtoMinusStudentRelations>();
    }
}
