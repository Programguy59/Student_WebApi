using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Contracts
{
    public interface IStudentCourseRepository : IRepositoryBase<StudentCourse>
    {
        #region From_StudentCourse
        Task<IEnumerable<StudentCourse>> GetAllCoursesWithStudentID(int StudentID);

        Task<IEnumerable<StudentCourse>> GetAllStudentsWithCourseID(int CourseID);

        Task<StudentCourse> GetStudentIDCourseIDCombination(int StudentID, int CourseID);
        #endregion
    }
}
