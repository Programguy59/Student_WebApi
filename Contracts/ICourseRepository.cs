using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICourseRepository : IRepositoryBase<Course>
    {
        // Filen her er kun medtaget for at åbne op for, at man kan placere "specielle"
        // funktioner vedrørende Course funktionalitet her. Ellers kan man styre det
        // hele med de generiske funktioner erklæret i IRepositiryBase.cs og implementeret i RepositoryBase.cs.
    }
}
