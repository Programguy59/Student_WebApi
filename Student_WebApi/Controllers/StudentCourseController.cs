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
    public class StudentCourseController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private ILoggerManager _logger;

#if Use_Hub_Logic_On_ServerSide
        private readonly IHubContext<BroadcastHub> _broadcastHub;
#endif

        public StudentCourseController(ILoggerManager logger,
                                       IRepositoryWrapper repositoryWrapper)
        {
            this._logger = logger;
            this._repositoryWrapper = repositoryWrapper;
        }

        [HttpGet("GetStudentCourses")]
        public async Task<IActionResult> GetStudentCourses(string UserName = "No Name",
                                                           bool UseLazyLoading = true)
        {
            try
            {
                IEnumerable<StudentCourse> StudentCourseList = new List<StudentCourse>();

                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();

                StudentCourseList = await this._repositoryWrapper.StudentCourseRepositoryWrapper.FindAll();

                List<StudentCourseDto> StudentCourseDtos;

                StudentCourseDtos = StudentCourseList.Adapt<StudentCourseDto[]>().ToList();

                this._logger.LogInfo($"All CityLangueges has been read from GetCiyLanguages action by {UserName}");
                return Ok(StudentCourseDtos);
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetStudntCourses action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetCoursesWithStudentID/{StudentID}")]
        public async Task<IActionResult> GetCoursesWithStudentID(int StudentID,
                                                                 string UserName = "No Name",
                                                                 bool UseLazyLoading = true)
        {
            try
            {
                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();

                IEnumerable<StudentCourse> StudentCourseList = new List<StudentCourse>();
                StudentCourseList = await this._repositoryWrapper.StudentCourseRepositoryWrapper.GetAllCoursesWithStudentID(StudentID);

                if (null == StudentCourseList)
                {
                    return NotFound();
                }
                else
                {
                    List<StudentCourseDto> StudentCourseDtos;

                    StudentCourseDtos = StudentCourseList.Adapt<StudentCourseDto[]>().ToList();

                    return Ok(StudentCourseDtos);
                }
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetCoursesWithStudentID action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetStudentsWithCourseID/{CourseID}")]
        public async Task<IActionResult> GetStudentsWithCourseID(int CourseID,
                                                                 string UserName = "No Name",
                                                                 bool UseLazyLoading = true)
        {
            try
            {
                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();

                IEnumerable<StudentCourse> StudentCourseList = new List<StudentCourse>();
                StudentCourseList = await this._repositoryWrapper.StudentCourseRepositoryWrapper.GetAllStudentsWithCourseID(CourseID);

                if (null == StudentCourseList)
                {
                    return NotFound();
                }
                else
                {
                    List<StudentCourseDto> StudentCourseDtos;

                    StudentCourseDtos = StudentCourseList.Adapt<StudentCourseDto[]>().ToList();

                    return Ok(StudentCourseDtos);
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside GetStudentsWithCourseID action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetStudentCourseWithStudentIDAndCourseID/{StudentID}/{CourseID}")]
        public async Task<IActionResult> GetStudentCourseWithStudentIDAndCourseID([FromRoute] int StudentID,
                                                                                  [FromRoute] int CourseID,
                                                                                  string UserName = "No Name",
                                                                                  bool UseLazyLoading = true)
        {
            try
            {
                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.StudentCourseRepositoryWrapper.EnableLazyLoading();

                StudentCourse StudentCourse_Object = await this._repositoryWrapper.StudentCourseRepositoryWrapper.GetStudentIDCourseIDCombination(StudentID, CourseID);

                if (null == StudentCourse_Object)
                {
                    return NotFound();
                }
                else
                {
                    StudentCourseDto StudentCourseDto_Object = StudentCourse_Object.Adapt<StudentCourseDto>();

                    return Ok(StudentCourseDto_Object);
                }
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetStudentCourseWithStudentIDAndCourseID action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // POST: api/StudentCourse
        [HttpPost("CreateStudentCourse")]
        public async Task<IActionResult> CreateStudentCourse([FromBody] StudentCourseForSaveDto StudentCourseForSaveDto_Object,
                                                              string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsSaved = 0;

                StudentCourse StudentCourse_Object = StudentCourseForSaveDto_Object.Adapt<StudentCourse>();

                await this._repositoryWrapper.StudentCourseRepositoryWrapper.Create(StudentCourse_Object);
                NumberOfObjectsSaved = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsSaved)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateStudentCourseDataMessage");
#endif
                    this._logger.LogInfo($"StudentCourse with StudentID : {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} has been stored by {UserName} !!!");
                    return Ok(StudentCourseForSaveDto_Object);
                }
                else
                {
                    _logger.LogError($"Error when saving StudentCourse with StudentID : {StudentCourse_Object.StudentID} and LanguageId {StudentCourse_Object.CourseID} by {UserName} !!!");
                    return BadRequest($"Error when saving StudentCourse with StudentID : {StudentCourse_Object.StudentID} and LanguageId {StudentCourse_Object.CourseID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside CreateStudentCourse action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // PUT: api/StudentCourse/5/1
        [HttpPut("UpdateStudentCourse/{OldStudentID}/{OldCourseID}")]
        public async Task<IActionResult> UpdateStudentCourse(int OldStudentID,
                                                            int OldCourseID,
                                                            [FromBody] StudentCourseForUpdateDto StudentCourseForUpdateDto_Object,
                                                            string UserName = "No Name")
        {
            using var Transaction = await _repositoryWrapper.GetCurrentDatabaseContext().Database.BeginTransactionAsync();

            try
            {
                int NumberOfObjectsUpdated = 0;
                int NumberOfObjectsDeleted = 0;

                await Transaction.CreateSavepointAsync("BeforeUpdate");

                StudentCourse StudentCourse_Object = await this._repositoryWrapper.StudentCourseRepositoryWrapper.GetStudentIDCourseIDCombination(OldStudentID, OldCourseID);

                if (null == StudentCourse_Object)
                {
                    return NotFound();
                }
                else
                {
                    await this._repositoryWrapper.StudentCourseRepositoryWrapper.Delete(StudentCourse_Object);

                    NumberOfObjectsDeleted = await this._repositoryWrapper.Save();

                    if (1 == NumberOfObjectsDeleted)
                    {
                        StudentCourse_Object = new StudentCourse();
                        TypeAdapter.Adapt(StudentCourseForUpdateDto_Object, StudentCourse_Object);

                        await this._repositoryWrapper.StudentCourseRepositoryWrapper.Create(StudentCourse_Object);

                        NumberOfObjectsUpdated = await this._repositoryWrapper.Save();

                        if (1 == NumberOfObjectsUpdated)
                        {
#if Use_Hub_Logic_On_ServerSide
                            await this._broadcastHub.Clients.All.SendAsync("UpdateStudentCourseDataMessage");
#endif
                            await Transaction.CommitAsync();
                            this._logger.LogInfo($"StudentCourse with StudentID : {StudentCourseForUpdateDto_Object.StudentID} and CourseID : {StudentCourseForUpdateDto_Object.CourseID} has been updated to {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} by {UserName} !!!");
                            return Ok(StudentCourseForUpdateDto_Object);
                        }
                        else
                        {
                            await Transaction.RollbackToSavepointAsync("BeforeUpdate");
                            this._logger.LogError($"Error when updating StudentCourse with StudentID : {StudentCourseForUpdateDto_Object.StudentID} and CourseID : {StudentCourseForUpdateDto_Object.CourseID} has been updated to {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} by {UserName} !!!");
                            return BadRequest($"Error when updating StudentCourse with StudentID : {StudentCourseForUpdateDto_Object.StudentID} and CourseID : {StudentCourseForUpdateDto_Object.CourseID} has been updated to {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} by {UserName} !!!");
                        }

                    }
                    else
                    {
                        await Transaction.RollbackToSavepointAsync("BeforeUpdate");
                        this._logger.LogError($"Error when deleting StudentCourse with StudentID : {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} by {UserName} !!!");
                        return BadRequest($"Error when deleting StudentCourse with StudentID : {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} by {UserName} !!!");
                    }
                }
            }
            catch (Exception Error)
            {
                await Transaction.RollbackToSavepointAsync("BeforeUpdate");
                _logger.LogError($"Something went wrong inside UpdateStudentCourse action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // DELETE: api/StudentCourse/5/1
        [HttpDelete("DeleteStudentCourse/{StudentID}/{CourseID}")]
        public async Task<IActionResult> DeleteStudentCourse(int StudentID,
                                                             int CourseID,
                                                             string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsDeleted;

                StudentCourse StudentCourse_Object = await this._repositoryWrapper.StudentCourseRepositoryWrapper.GetStudentIDCourseIDCombination(StudentID, CourseID);

                if (null == StudentCourse_Object)
                {
                    return NotFound();
                }

                await this._repositoryWrapper.StudentCourseRepositoryWrapper.Delete(StudentCourse_Object);

                NumberOfObjectsDeleted = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsDeleted)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateStudentCourseDataMessage");
#endif
                    this._logger.LogInfo($"StudentCourse with StudentID : {StudentCourse_Object.StudentID} and CourseID : {StudentCourse_Object.CourseID} has been deleted by {UserName} !!!");
                    return Ok($"StudentCourse with StudentID : {StudentCourse_Object.StudentID} and CourseID : {StudentCourse_Object.CourseID} has been deleted by {UserName} !!!");
                }
                else
                {
                    _logger.LogError($"Error when deleting StudentCourse with StudentID : {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} by {UserName} !!!");
                    return BadRequest($"Error when deleting StudentCourse with StudentID : {StudentCourse_Object.StudentID} and CourseID {StudentCourse_Object.CourseID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside DeleteStudentCourse action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }
    }
}
