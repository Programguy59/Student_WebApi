using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Repository
{
    public class StudentCourseRepository : RepositoryBase<StudentCourse>, IStudentCourseRepository
    {
        #region General
        private readonly DatabaseContext _context;

        public StudentCourseRepository(DatabaseContext context) : base(context) 
        { 
           _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region From_StudentCourse
        public async Task<IEnumerable<StudentCourse>> GetAllCoursesWithStudentID(int StudentID)
        {
            var collection = await base.FindByCondition(sc => sc.StudentID == StudentID);
            //collection = collection.OrderByDescending(sc => sc.Student.StudentCourses.Count).ThenBy(c => c.Course.CourseName);

            return (collection.ToList());
        }

        public async Task<IEnumerable<StudentCourse>> GetAllStudentsWithCourseID(int CourseID)
        {
            var collection = await base.FindByCondition(sc => sc.CourseID == CourseID);
            //collection = collection.OrderByDescending(sc => sc.Student.StudentCourses.Count);

            return (collection.ToList());
        }

        public async Task<StudentCourse> GetStudentIDCourseIDCombination(int StudentID, int CourseID)
        {
            var StudentCourseItem = await base.FindByCondition(sc => sc.StudentID == StudentID &&
                                                               sc.CourseID == CourseID);

            if (null != StudentCourseItem)
            {
                return (StudentCourseItem.ElementAt(0));
            }
            else
            {
                return (null);
            }

        }

        //public async Task<IEnumerable<CityLanguage>> GetAllCitiesLanguages(bool IncludeRelations = false)
        //{
        //    if (false == IncludeRelations)
        //    {
        //        var collection = await (base.FindAll());
        //        collection = collection.OrderByDescending(c => c.City.CityLanguages.Count);
        //        return (collection);
        //    }
        //    else
        //    {
        //        base.DisableLazyLoading();
        //        var collection = await base.databaseContext.Core_8_0_CityLanguages.
        //            Include(c => c.City).
        //            ThenInclude(p => p.PointsOfInterest).
        //            Include(c => c.City).
        //            ThenInclude(co => co.Country).
        //            Include(l => l.Language).ToListAsync();

        //        var collection1 = collection.OrderByDescending(c => c.City.CityLanguages.Count);

        //        return collection1;
        //    }
        //}

        //public async Task<IEnumerable<CityLanguage>> GetAllCitiesFromLanguageId(int LanguageId)
        //{
        //    var collection = await _context.Core_8_0_CityLanguages.Where(l => l.LanguageId == LanguageId).ToListAsync();
        //        //as IQueryable<CityLanguage>;

        //    var collection1 = collection.OrderByDescending(c => c.City.CityLanguages.Count);

        //    return collection1;
        //}

        //public async Task<IEnumerable<CityLanguage>> GetAllLanguagesFromCityId(int CityId)
        //{
        //    var collection = await _context.Core_8_0_CityLanguages.Where(c => c.CityId == CityId).
        //        Include(c => c.City).
        //        Include(l => l.Language).ToListAsync();
        //        //as IQueryable<CityLanguage>;

        //    var collection1 = collection.OrderByDescending(l => l.Language.CityLanguages.Count);

        //    return collection1;
        //}

        //public async Task AddCityLanguage(CityLanguage cityLanguage)
        //{
        //    await _context.Core_8_0_CityLanguages.AddAsync(cityLanguage);
        //}

        //// Kode fra nyt generisk interface herunder.
        //public async Task<IEnumerable<CityLanguage>> GetAllCitiesWithLanguageId(int LanguageId)
        //{
        //    var collection = await base.FindByCondition(l => l.LanguageId == LanguageId);
        //    collection = collection.OrderByDescending(l => l.Language.CityLanguages.Count).ThenBy(c => c.City.CityName);

        //    return (collection.ToList());
        //}

        //public async Task<IEnumerable<CityLanguage>> GetAllLanguagesWithCityId(int CityId)
        //{
        //    var collection = await base.FindByCondition(l => l.CityId == CityId);
        //    collection = collection.OrderByDescending(c => c.City.CityLanguages.Count).ThenBy(l => l.Language.LanguageName);

        //    return (collection.ToList());
        //}

        //public async Task<CityLanguage> GetCityIdLanguageIdCombination(int CityId, int LanguageId)
        //{
        //    var CityLanguageItem = await base.FindByCondition(l => l.LanguageId == LanguageId &&
        //                                                      l.CityId == CityId);

        //    if (null != CityLanguageItem)
        //    {
        //        return (CityLanguageItem.ElementAt(0));
        //    }
        //    else
        //    {
        //        return (null);
        //    }

        //}

        //public async Task<bool> UpdateCityLanguageCombination(CityLanguage CityLanguageToDelete_Object,
        //                                                      CityLanguage CityLanguageToSave_Object)
        //{
        //    try
        //    {
        //        await base.Delete(CityLanguageToDelete_Object);

        //        await base.Create(CityLanguageToSave_Object);

        //        return (true);
        //    }
        //    catch (Exception Error)
        //    {
        //        return (false);
        //    }
        //}

        //public async Task<bool> UpdateCityLanguageList(List<CityLanguageForSaveAndUpdateDto> CityLanguageNew_Object_list,
        //                                               List<CityLanguageDto> CityLanguageOld_Object_List)
        //{
        //    int ListCounter;

        //    for (ListCounter = 0; ListCounter < CityLanguageOld_Object_List.Count; ListCounter++)
        //    {

        //    }

        //    try
        //    {
        //        //await base.Delete(CityLanguageToDelete_Object);

        //        //await base.Create(CityLanguageToSave_Object);

        //        return (true);
        //    }
        //    catch (Exception Error)
        //    {
        //        return (false);
        //    }
        //}

        //public bool LanguageIdFoundInCityLanguageList(List<CityLanguageForSaveAndUpdateDto> CityLanguageNew_Object_list,
        //                                              int LanguageId)
        //{
        //    int ListCounter;

        //    for (ListCounter = 0; ListCounter < CityLanguageNew_Object_list.Count; ListCounter++)
        //    {
        //        if (LanguageId == CityLanguageNew_Object_list[ListCounter].LanguageId)
        //        {
        //            return (true);
        //        }
        //    }

        //    return (false);
        //}
        #endregion
    }
}
