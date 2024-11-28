using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{

    public class TeamForSaveDto
    {
        public string TeamName { get; set; }
    }

    public class TeamForUpdateDto : TeamForSaveDto
    {
        public int TeamID { get; set; }
    }

    public class TeamDtoMinusRelations : TeamForUpdateDto
    {
    }

    public class TeamDto : TeamDtoMinusRelations
    {
        public ICollection<StudentDtoMinusRelations> Students { get; set; }
              = new List<StudentDtoMinusRelations>();
    }

}
