using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    // Database model
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string ?CourseName { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
               = new List<StudentCourse>();
    }
 
}
