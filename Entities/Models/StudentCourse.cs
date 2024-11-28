using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class StudentCourse
    {
        [Required]
        [Key]
        public int StudentID { get; set; }
     
        [Required]
        [Key]
        public int CourseID { get; set; }

        public virtual Student ?Student { get; set; }

        public virtual Course ?Course { get; set; }
    }
}
