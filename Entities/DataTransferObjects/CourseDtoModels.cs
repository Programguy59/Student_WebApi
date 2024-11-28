using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class CourseForSaveDto
    {
        public string ?CourseName { get; set; }
    }

    public class CourseForUpdateDto : CourseForSaveDto
    {
        public int CourseID { get; set; }
    }

    public class CourseDtoMinusRelations : CourseForUpdateDto
    {

    }

    public class CourseDto : CourseForUpdateDto
    {
        public ICollection<StudentCourseDtoMinusCourseRelations> StudentCourses { get; set; }
               = new List<StudentCourseDtoMinusCourseRelations>();
    }
}
