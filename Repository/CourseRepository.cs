using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        private readonly DatabaseContext _context;

        public CourseRepository(DatabaseContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
