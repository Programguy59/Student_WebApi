using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Course_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private ILoggerManager _logger;

#if Use_Hub_Logic_On_ServerSide
        private readonly IHubContext<BroadcastHub> _broadcastHub;
#endif
        public CourseController(ILoggerManager logger,
                                 IRepositoryWrapper repositoryWrapper)
        {
            this._logger = logger;
            this._repositoryWrapper = repositoryWrapper;
        }

        [HttpGet("GetCourses")]
        public async Task<IActionResult> GetCourses(string UserName = "No Name",
                                                    bool UseLazyLoading = true)
        {
            try
            {
                IEnumerable<Course> CourseList = new List<Course>();

                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.CourseRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.CourseRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.CourseRepositoryWrapper.EnableLazyLoading();

                CourseList = await this._repositoryWrapper.CourseRepositoryWrapper.FindAll();

                List<CourseDto> CourseDtoList;

                CourseDtoList = CourseList.Adapt<CourseDto[]>().ToList();

                this._logger.LogInfo($"All Courses have been read from GetCourses action by {UserName}");
                return Ok(CourseDtoList);
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetCourses action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetCourse/{CourseID}")]
        public async Task<IActionResult> GetCourse(int CourseID,
                                                   string UserName = "No Name",
                                                   bool UseLazyLoading = true)
        {
            try
            {
                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.CourseRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.CourseRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.CourseRepositoryWrapper.EnableLazyLoading();

                Course Course_Object = await this._repositoryWrapper.CourseRepositoryWrapper.FindOne(CourseID);

                if (null == Course_Object)
                {
                    return NotFound();
                }
                else
                {
                    CourseDto CourseDto_Object = Course_Object.Adapt<CourseDto>();
                    this._logger.LogInfo($"Course with CourseID {CourseID} has been read from GetCourse action by {UserName}");
                    return Ok(CourseDto_Object);
                }
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetCourse action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // POST: api/Course
        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseForSaveDto CourseForSaveDto_Object,
                                                       string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsSaved = 0;

                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateCourse");
                    return BadRequest(ModelState);
                }

                Course Course_Object = CourseForSaveDto_Object.Adapt<Course>();

                await this._repositoryWrapper.CourseRepositoryWrapper.Create(Course_Object);
                NumberOfObjectsSaved = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsSaved)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateCityDataMessage");
#endif
                    _logger.LogInfo($"Course with ID : {Course_Object.CourseID} has been saved by {UserName} !!!");
                    return Ok(Course_Object.CourseID);
                }
                else
                {
                    _logger.LogError($"Error when saving Course by {UserName} !!!");
                    return BadRequest($"Error when saving Course by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside CreateCourse action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // PUT: api/Course/5
        [HttpPut("UpdateCourse/{CourseID}")]
        public async Task<IActionResult> UpdateCourse(int CourseID,
                                                       [FromBody] CourseForUpdateDto CourseForUpdateDto_Object,
                                                       string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsUpdated = 0;

                if (CourseID != CourseForUpdateDto_Object.CourseID)
                {
                    _logger.LogError($"CourseID != CourseForUpdateDto_Object.CourseID for {UserName} in action UpdateCourse");
                    return BadRequest($"CourseID != CourseForUpdateDto_Object.CourseID for {UserName} in action UpdateCourse");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"ModelState is Invalid for {UserName} in action UpdateCourse");
                    return BadRequest(ModelState);
                }

                Course Course_Object = await _repositoryWrapper.CourseRepositoryWrapper.FindOne(CourseID);

                if (null == Course_Object)
                {
                    return NotFound();
                }

                TypeAdapter.Adapt(CourseForUpdateDto_Object, Course_Object);

                await _repositoryWrapper.CourseRepositoryWrapper.Update(Course_Object);

                NumberOfObjectsUpdated = await _repositoryWrapper.Save();

                if (1 == NumberOfObjectsUpdated)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateCourseDataMessage");
#endif
                    _logger.LogInfo($"Course with ID : {Course_Object.CourseID} has been updated by {UserName} !!!");
                    return Ok($"Course with ID : {Course_Object.CourseID} has been updated by {UserName} !!!"); ;
                }
                else
                {
                    _logger.LogError($"Error when updating Course with ID : {Course_Object.CourseID} by {UserName} !!!");
                    return BadRequest($"Error when updating Course with ID : {Course_Object.CourseID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside UpdateCourse action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // DELETE: api/Course/5
        [HttpDelete("DeleteCourse/{CourseID}")]
        public async Task<IActionResult> DeleteCourse(int CourseID,
                                                       string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsDeleted;

                Course Course_Object = await this._repositoryWrapper.CourseRepositoryWrapper.FindOne(CourseID);

                if (null == Course_Object)
                {
                    this._logger.LogError($"Course with ID {CourseID} not found inside action DeleteCourse for {UserName}");
                    return NotFound();
                }

                await this._repositoryWrapper.CourseRepositoryWrapper.Delete(Course_Object);

                NumberOfObjectsDeleted = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsDeleted)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateCourseDataMessage");
#endif
                    this._logger.LogInfo($"Course with ID {CourseID} has been deleted in action DeleteCourse by {UserName}");
                    return Ok($"Course with ID {CourseID} has been deleted in action DeleteCourse by {UserName}");
                }
                else
                {
                    _logger.LogError($"Error when deleting Course with ID : {CourseID} by {UserName} !!!");
                    return BadRequest($"Error when deleting Course with ID : {CourseID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside DeleteCourse action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }
    }
}
