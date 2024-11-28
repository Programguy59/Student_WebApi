using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private ILoggerManager _logger;

#if Use_Hub_Logic_On_ServerSide
    private readonly IHubContext<BroadcastHub> _broadcastHub;
#endif
        public TeamController(ILoggerManager logger,
                              IRepositoryWrapper repositoryWrapper)
        {
            this._logger = logger;
            this._repositoryWrapper = repositoryWrapper;
        }

        [HttpGet("GetTeams")]
        public async Task<IActionResult> GetTeams(string UserName = "No Name",
                                                  bool UseLazyLoading = true)
        {
            try
            {
                IEnumerable<Team> TeamList = new List<Team>();

                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.TeamRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.TeamRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.TeamRepositoryWrapper.EnableLazyLoading();

                TeamList = await this._repositoryWrapper.TeamRepositoryWrapper.FindAll();

                List<TeamDto> TeamDtoList;

                TeamDtoList = TeamList.Adapt<TeamDto[]>().ToList();

                this._logger.LogInfo($"All Teams have been read from GetTeams action by {UserName}");
                return Ok(TeamDtoList);
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetTeams action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetTeam/{TeamID}")]
        public async Task<IActionResult> GetTeam(int TeamID,
                                                 string UserName = "No Name",
                                                 bool UseLazyLoading = true)
        {
            try
            {
                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.TeamRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.TeamRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.TeamRepositoryWrapper.EnableLazyLoading();

                Team Team_Object = await this._repositoryWrapper.TeamRepositoryWrapper.FindOne(TeamID);

                if (null == Team_Object)
                {
                    return NotFound();
                }
                else
                {
                    TeamDto TeamDto_Object = Team_Object.Adapt<TeamDto>();
                    this._logger.LogInfo($"Team with TeamID {TeamID} has been read from GetTeam action by {UserName}");
                    return Ok(TeamDto_Object);
                }
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetTeam action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // POST: api/Team
        [HttpPost("CreateTeam")]
        public async Task<IActionResult> CreateTeam([FromBody] TeamForSaveDto TeamForSaveDto_Object,
                                                    string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsSaved = 0;

                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateTeam");
                    return BadRequest(ModelState);
                }

                Team Team_Object = TeamForSaveDto_Object.Adapt<Team>();

                await this._repositoryWrapper.TeamRepositoryWrapper.Create(Team_Object);
                NumberOfObjectsSaved = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsSaved)
                {
#if Use_Hub_Logic_On_ServerSide
                await this._broadcastHub.Clients.All.SendAsync("UpdateCityDataMessage");
#endif
                    _logger.LogInfo($"Team with ID : {Team_Object.TeamID} has been saved by {UserName} !!!");
                    return Ok($"Team with ID : {Team_Object.TeamID} has been added by {UserName} !!!");
                }
                else
                {
                    _logger.LogError($"Error when saving Team by {UserName} !!!");
                    return BadRequest($"Error when saving Team by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside CreateTeam action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // PUT: api/Team/5
        [HttpPut("UpdateTeam/{TeamID}")]
        public async Task<IActionResult> UpdateTeam(int TeamID,
                                                    [FromBody] TeamForUpdateDto TeamForUpdateDto_Object,
                                                    string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsUpdated = 0;

                if (TeamID != TeamForUpdateDto_Object.TeamID)
                {
                    _logger.LogError($"TeamID != TeamForUpdateDto_Object.TeamID for {UserName} in action UpdateTeam");
                    return BadRequest($"TeamID != TeamForUpdateDto_Object.TeamID for {UserName} in action UpdateTeam");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"ModelState is Invalid for {UserName} in action UpdateTeam");
                    return BadRequest(ModelState);
                }

                Team Team_Object = await _repositoryWrapper.TeamRepositoryWrapper.FindOne(TeamID);

                if (null == Team_Object)
                {
                    return NotFound();
                }

                TypeAdapter.Adapt(TeamForUpdateDto_Object, Team_Object);

                await _repositoryWrapper.TeamRepositoryWrapper.Update(Team_Object);

                NumberOfObjectsUpdated = await _repositoryWrapper.Save();

                if (1 == NumberOfObjectsUpdated)
                {
#if Use_Hub_Logic_On_ServerSide
                await this._broadcastHub.Clients.All.SendAsync("UpdateTeamDataMessage");
#endif
                    _logger.LogInfo($"Team with ID : {Team_Object.TeamID} has been updated by {UserName} !!!");
                    return Ok($"Team with ID : {Team_Object.TeamID} has been updated by {UserName} !!!");
                }
                else
                {
                    _logger.LogError($"Error when updating Team with ID : {Team_Object.TeamID} by {UserName} !!!");
                    return BadRequest($"Error when updating Team with ID : {Team_Object.TeamID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside UpdateTeam action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // DELETE: api/Team/5
        [HttpDelete("DeleteTeam/{TeamID}")]
        public async Task<IActionResult> DeleteTeam(int TeamID,
                                                    string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsDeleted;

                Team Team_Object = await this._repositoryWrapper.TeamRepositoryWrapper.FindOne(TeamID);

                if (null == Team_Object)
                {
                    this._logger.LogError($"Team with ID {TeamID} not found inside action DeleteTeam for {UserName}");
                    return NotFound();
                }

                await this._repositoryWrapper.TeamRepositoryWrapper.Delete(Team_Object);

                NumberOfObjectsDeleted = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsDeleted)
                {
#if Use_Hub_Logic_On_ServerSide
                await this._broadcastHub.Clients.All.SendAsync("UpdateTeamDataMessage");
#endif
                    this._logger.LogInfo($"Team with ID {TeamID} has been deleted in action DeleteTeam by {UserName}");
                    return Ok($"Team with ID {TeamID} has been deleted in action DeleteTeam by {UserName}");
                }
                else
                {
                    _logger.LogError($"Error when deleting Team with ID : {TeamID} by {UserName} !!!");
                    return BadRequest($"Error when deleting Team with ID : {TeamID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside DeleteTeam action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }
    }
}
