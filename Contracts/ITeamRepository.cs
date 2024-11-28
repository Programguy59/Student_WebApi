using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ITeamRepository : IRepositoryBase<Team>
    {
        // Filen her er kun medtaget for at åbne op for, at man kan placere "specielle"
        // funktioner vedrørende Team funktionalitet her. Ellers kan man styre det
        // hele med de generiske funktioner erklæret i IRepositoryBase.cs og implementeret
        // i RepositoryBase.cs.
    }
}
