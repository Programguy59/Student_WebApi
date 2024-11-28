using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        public DatabaseContext GetCurrentDatabaseContext();

        IStudentRepository StudentRepositoryWrapper { get; }

        ICourseRepository CourseRepositoryWrapper { get; }

        IStudentCourseRepository StudentCourseRepositoryWrapper { get; }

        ITeamRepository TeamRepositoryWrapper { get; }

        Task<int> Save();

    }
}
