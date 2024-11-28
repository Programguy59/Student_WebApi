using Entities;
using Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    // Wrapper funktionaliten i RepositoryWrapper filen her ses også
    // benævnt UnitOfWork i andre sammenhænge.
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DatabaseContext _databaseContext;
       
        private IStudentRepository? _studentRepositoryWrapper;
        private IStudentCourseRepository? _studentCourseRepositoryWrapper;
        private ICourseRepository? _courseRepositoryWrapper;
        private ITeamRepository? _teamRepositoryWrapper;
        
        public RepositoryWrapper(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public DatabaseContext GetCurrentDatabaseContext()
        {
           return (this._databaseContext);
        }

        public IStudentRepository StudentRepositoryWrapper
        {
            get
            {
                if (null == _studentRepositoryWrapper)
                {
                    _studentRepositoryWrapper = new StudentRepository(this._databaseContext);
                }

                return (_studentRepositoryWrapper);
            }
        }

        public IStudentCourseRepository StudentCourseRepositoryWrapper
        {
            get
            {
                if (null == _studentCourseRepositoryWrapper)
                {
                    _studentCourseRepositoryWrapper = new StudentCourseRepository(this._databaseContext);
                }

                return (_studentCourseRepositoryWrapper);
            }
        }

        public ICourseRepository CourseRepositoryWrapper
        {
            get
            {
                if (null == _courseRepositoryWrapper)
                {
                    _courseRepositoryWrapper = new CourseRepository(this._databaseContext);
                }

                return (_courseRepositoryWrapper);
            }
        }

        public ITeamRepository TeamRepositoryWrapper
        {
            get
            {
                if (null == _teamRepositoryWrapper)
                {
                    _teamRepositoryWrapper = new TeamRepository(this._databaseContext);
                }

                return (_teamRepositoryWrapper);
            }
        }

        public async Task<int> Save()
        {
          int NumberOfObjectsChanged = 0;
          NumberOfObjectsChanged = await this._databaseContext.SaveChangesAsync();

          return (NumberOfObjectsChanged);
        }
    }
}
