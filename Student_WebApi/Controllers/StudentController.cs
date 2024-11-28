using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IRepositoryWrapper _repositoryWrapper;
        private ILoggerManager _logger;

#if Use_Hub_Logic_On_ServerSide
        private readonly IHubContext<BroadcastHub> _broadcastHub;
#endif
        public StudentController(ILoggerManager logger,
                                 IRepositoryWrapper repositoryWrapper)
        {
            this._logger = logger;
            this._repositoryWrapper = repositoryWrapper;
        }

        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetStudents(string UserName = "No Name",
                                                     bool UseLazyLoading = true)
        {
            try
            {
                IEnumerable<Student> StudentList = new List<Student>();

                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.StudentRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();
                
                StudentList = await this._repositoryWrapper.StudentRepositoryWrapper.FindAll();

                List<StudentDto> StudentDtoList;

                StudentDtoList = StudentList.Adapt<StudentDto[]>().ToList();

                this._logger.LogInfo($"All Students have been read from GetStudents action by {UserName}");
                return Ok(StudentDtoList);
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetStudents action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetStudent/{StudentID}")]
        public async Task<IActionResult> GetStudent(int StudentID,
                                                    string UserName = "No Name",
                                                    bool UseLazyLoading = true)
        {
            try
            {
                if (false == UseLazyLoading)
                {
                    _repositoryWrapper.StudentRepositoryWrapper.DisableLazyLoading();
                }
                else
                {
                    _repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();
                }
                //this._repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();

                Student Student_Object = await this._repositoryWrapper.StudentRepositoryWrapper.FindOne(StudentID);

                if (null == Student_Object)
                {
                    return NotFound();
                }
                else
                {
                    StudentDto StudentDto_Object = Student_Object.Adapt<StudentDto>();
                    this._logger.LogInfo($"Student with StudentID {StudentID} has been read from GetStudent action by {UserName}");
                    return Ok(StudentDto_Object);
                }
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetStudent action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetStudentsInTeam/{TeamID}")]
        public async Task<IActionResult> GetStudentsInTeam(int TeamID,
                                                           string UserName = "No Name")
        {
            try
            {
              IEnumerable<Student> StudentList = new List<Student>();

              this._repositoryWrapper.StudentRepositoryWrapper.EnableLazyLoading();

              StudentList = await this._repositoryWrapper.StudentRepositoryWrapper.GetStudentsInTeam(TeamID);

              List<StudentDto> StudentDtoList;

              StudentDtoList = StudentList.Adapt<StudentDto[]>().ToList();

              this._logger.LogInfo($"All Students within TeamID : {TeamID} has been read from GetStudentsInTeam action by {UserName}");
              return Ok(StudentDtoList);
            }
            catch (Exception Error)
            {
              this._logger.LogError($"Something went wrong inside GetStudentsInTeam action for {UserName} : {Error.Message}");
              return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // POST: api/Student
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentForSaveDto StudentForSaveDto_Object,
                                                       string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsSaved = 0;
                
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateStudent");
                    return BadRequest(ModelState);
                }

                Student Student_Object = StudentForSaveDto_Object.Adapt<Student>();

                await this._repositoryWrapper.StudentRepositoryWrapper.Create(Student_Object);
                NumberOfObjectsSaved = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsSaved)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateCityDataMessage");
#endif
                    _logger.LogInfo($"Student with ID : {Student_Object.StudentID} has been saved by {UserName} !!!");
                    return Ok($"Student with ID : {Student_Object.StudentID} has been saved by {UserName} !!!");
                }
                else
                {
                    _logger.LogError($"Error when saving Student by {UserName} !!!");
                    return BadRequest($"Error when saving Student by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside CreateStudent action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // PUT: api/Student/5
        [HttpPut("UpdateStudent/{StudentID}")]
        public async Task<IActionResult> UpdateStudent(int StudentID,
                                                       [FromBody] StudentForUpdateDto StudentForUpdateDto_Object,
                                                       string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsUpdated = 0;

                if (StudentID != StudentForUpdateDto_Object.StudentID)
                {
                    _logger.LogError($"StudentID != StudentForUpdateDto_Object.StudentID for {UserName} in action UpdateStudent");
                    return BadRequest($"StudentID != StudentForUpdateDto_Object.StudentID for {UserName} in action UpdateStudent");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"ModelState is Invalid for {UserName} in action UpdateStudent");
                    return BadRequest(ModelState);
                }

                Student Student_Object = await _repositoryWrapper.StudentRepositoryWrapper.FindOne(StudentID);

                if (null == Student_Object)
                {
                    return NotFound();
                }

                TypeAdapter.Adapt(StudentForUpdateDto_Object, Student_Object);

                await _repositoryWrapper.StudentRepositoryWrapper.Update(Student_Object);

                NumberOfObjectsUpdated = await _repositoryWrapper.Save();

                if (1 == NumberOfObjectsUpdated)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateStudentDataMessage");
#endif
                    _logger.LogInfo($"Student with ID : {Student_Object.StudentID} has been updated by {UserName} !!!");
                    return Ok($"Student with ID : {Student_Object.StudentID} has been updated by {UserName} !!!"); ;
                }
                else
                {
                    _logger.LogError($"Error when updating Student with ID : {Student_Object.StudentID} by {UserName} !!!");
                    return BadRequest($"Error when updating Student with ID : {Student_Object.StudentID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside UpdateStudent action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // DELETE: api/Student/5
        [HttpDelete("DeleteStudent/{StudentID}")]
        public async Task<IActionResult> DeleteStudent(int StudentID,
                                                       string UserName = "No Name")
        {
            try
            {
                int NumberOfObjectsDeleted;

                Student Student_Object = await this._repositoryWrapper.StudentRepositoryWrapper.FindOne(StudentID);

                if (null == Student_Object)
                {
                    this._logger.LogError($"Student with ID {StudentID} not found inside action DeleteStudent for {UserName}");
                    return NotFound();
                }

                await this._repositoryWrapper.StudentRepositoryWrapper.Delete(Student_Object);

                NumberOfObjectsDeleted = await this._repositoryWrapper.Save();

                if (1 == NumberOfObjectsDeleted)
                {
#if Use_Hub_Logic_On_ServerSide
                    await this._broadcastHub.Clients.All.SendAsync("UpdateStudentDataMessage");
#endif
                    this._logger.LogInfo($"Student with ID {StudentID} has been deleted in action DeleteStudent by {UserName}");
                    return Ok($"Student with ID {StudentID} has been deleted in action DeleteStudent by {UserName}");
                }
                else
                {
                    _logger.LogError($"Error when deleting Student with ID : {StudentID} by {UserName} !!!");
                    return BadRequest($"Error when deleting Student with ID : {StudentID} by {UserName} !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside DeleteStudent action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }
    }
}
