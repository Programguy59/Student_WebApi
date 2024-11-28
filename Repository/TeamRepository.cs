using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        private readonly DatabaseContext _context;

        public TeamRepository(DatabaseContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
