using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DatabaseContext : DbContext
    {
        private static string _sQLConnectionString = String.Empty;
        private readonly IConfiguration _configuration;

        public static string SQLConnectionString
        {
            get
            {
                return _sQLConnectionString;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _sQLConnectionString = value;
                }
            }
        }

        //private readonly IConfiguration _configuration;
        public virtual DbSet<Student> Core_8_0_Students { get; set; }
        public virtual DbSet<Course> Core_8_0_Courses { get; set; }
        public virtual DbSet<Team> Core_8_0_Teams { get; set; }

       public virtual DbSet<StudentCourse> Core_8_0_StudentCourses { get; set; }


        // Constructor herunder bliver kaldt under normal kørsel.
        public DatabaseContext(DbContextOptions<DatabaseContext> options,
                               IConfiguration configuration) : base(options)
        {
            this._configuration = configuration;
        }
        public DatabaseContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(cl => new
                {
                    cl.StudentID,
                    cl.CourseID
                });

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString;

#if ENABLED_FOR_LAZY_LOADING_USAGE
            if (!String.IsNullOrEmpty(_sQLConnectionString))
            {
                connectionString = _sQLConnectionString;
            }
            else
            {
                connectionString = this._configuration.GetConnectionString("Student_WebApiDBConnectionString");
            }
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);
#endif
        }

    }
}
