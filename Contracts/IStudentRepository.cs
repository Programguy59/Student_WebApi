using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Contracts
{
    public interface IStudentRepository : IRepositoryBase<Student>
    {
        // Metoderne herunder er kun med for test formål. For at vise i 
        // CityController.cs hvordan man skal gøre for at få alle relationelle
        // data med, hvis man ikke har enabled lazy loading.

        Task<IEnumerable<Student>> GetStudentsInTeam(int TeamID);
        //Task<IEnumerable<City>> GetAllCities(bool IncludeRelations = false);
        //Task<IEnumerable<City>> GetAllCitiesMockable(bool IncludeRelations = false);

        //Task<City> GetCity(int CityId, bool IncludeRelations = false);

        //Task <IEnumerable<City>> GetCitiesFromLanguageID(int languageID);

        //Task<IEnumerable<City>> GetSpecifiedNumberOfCities(int NumberOfCities = 5,
        //                                                   bool IncludeRelations = false,
        //                                                   bool UseIQueryable = false);

        //Task<IEnumerable<City>> GetCitiesInCountry(int CountryId);
    }
}
