using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
  public class StudentRepository : RepositoryBase<Student>, IStudentRepository
  {
    public StudentRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
      if (null == databaseContext)
      {
        throw new ArgumentNullException(nameof(databaseContext));
      }
    }

    public async Task<IEnumerable<Student>> GetStudentsInTeam(int TeamID)
    {
        IEnumerable<Student> StudentList = new List<Student>();

        StudentList = await base.FindByCondition(s => s.TeamID == TeamID);

        return (StudentList);
    }

        //    public async Task<IEnumerable<City>> GetAllCitiesMockable(bool IncludeRelations = false)
        //    {
        //      if (false == IncludeRelations)
        //      {
        //        var collection = await (base.FindAll());
        //        return (collection);
        //      }
        //      else
        //      {
        //        var collection = await base.databaseContext.Core_8_0_Cities.
        //        Include(c => c.PointsOfInterest).
        //        Include(co => co.Country).
        //        Include(c => c.CityLanguages).
        //        ThenInclude(l => l.Language).ToListAsync();
        //        return (collection);
        //      }
        //    }

        //    public async Task<IEnumerable<City>> GetAllCities(bool IncludeRelations = false)
        //    {
        //      if (false == IncludeRelations)
        //      {
        //        try
        //        {
        //          var collection = await (base.FindAll());
        //          return (collection);
        //        }
        //        catch (Exception Error)
        //        {
        //          string ErrorString = Error.ToString();
        //          return null;
        //        }
        //      }
        //      else
        //      {
        //        var collection = await base.databaseContext.Core_8_0_Cities.
        //        Include(c => c.PointsOfInterest).
        //        Include(co => co.Country).
        //        Include(c => c.CityLanguages).
        //        ThenInclude(l => l.Language).ToListAsync();
        //        return (collection);
        //      }
        //    }

        //    public async Task<City> GetCity(int CityId, bool IncludeRelations = false)
        //    {
        //      if (false == IncludeRelations)
        //      {
        //        var City_Object = await base.FindOne(CityId);
        //        return (City_Object);
        //      }
        //      else
        //      {
        //        var City_Object = await base.databaseContext.Core_8_0_Cities.Include(c => c.PointsOfInterest).
        //        Include(c => c.PointsOfInterest).
        //        Include(c => c.CityLanguages).
        //        ThenInclude(l => l.Language).
        //        FirstOrDefaultAsync(c => c.CityId == CityId);

        //        return (City_Object);
        //      }
        //    }

        //#if OLD_IMPLEMENTATION
        //        public Async Task <IEnumerable<City>> GetCitiesFromLanguages(int languageID)
        //        {
        //            return RepositoryContext.Cities.Include(x => x.CityLanguages).ThenInclude(x => x.Language).Include(x => x.PointsOfInterest).Where(x => x.CityLanguages.Any(cl => cl.LanguageId == languageID));
        //        }
        //#else
        //    public async Task<IEnumerable<City>> GetCitiesFromLanguageID(int languageID)
        //    {
        //      var collection = await base.FindByCondition(x => x.CityLanguages.Any(cl => cl.LanguageId == languageID));

        //      collection = collection.OrderByDescending(c => c.CityLanguages.Count).ThenBy(c => c.CityName);

        //      return (collection.ToList());
        //    }
        //#endif
        //    public async Task<IEnumerable<City>> GetSpecifiedNumberOfCities(int NumberOfCities = 5,
        //                                                                    bool IncludeRelations = false,
        //                                                                    bool UseIQueryable = false)
        //    {
        //      IEnumerable<City> CityList = new List<City>();
        //      IEnumerable<City> CityListToReturn = new List<City>();

        //      if (false == IncludeRelations)
        //      {
        //        CityList = await base.FindByCondition(c => c.CityId > 0, UseIQueryable);
        //        CityListToReturn = CityList.Take(NumberOfCities);
        //      }
        //      else
        //      {
        //        base.EnableLazyLoading();
        //        CityList = await base.FindByCondition(c => c.CityId > 0, UseIQueryable);
        //        CityListToReturn = CityList.Take(NumberOfCities);
        //      }

        //      return (CityListToReturn);
        //    }

        //    public async Task<IEnumerable<City>> GetCitiesInCountry(int CountryId)
        //    {
        //      IEnumerable<City> CityList = new List<City>();

        //      CityList = await base.FindByCondition(c => c.CountryID == CountryId);

        //      return (CityList);
        //    }

    }
}  
