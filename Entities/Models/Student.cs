using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Student : ORM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }

        [Required]
        [MaxLength(100)]
        public string ?StudentName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ?StudentLastName { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
               = new List<StudentCourse>();
        
        [ForeignKey("TeamID")]
        public int TeamID { get; set; }

        public virtual Team Team { get; set; }

        // ADO Net funktionalitet herunder !!!

        public const string TABLE_NAME = "Core_8_0_Students";
        public override string TableName() { return TABLE_NAME; }

        static Student()
        {
            Int(TABLE_NAME, "StudentID", (orm) => (orm as Student).StudentID + "");
            Int(TABLE_NAME, "StudentName", (orm) => (orm as Student).StudentName);
            Int(TABLE_NAME, "StudentLastName", (orm) => (orm as Student).StudentLastName);
            Int(TABLE_NAME, "TeamID", (orm) => (orm as Student).TeamID + "");
            PrimaryKey(TABLE_NAME, "StudentID");
        }

        public Student()
        {
            
        }
    }
}
