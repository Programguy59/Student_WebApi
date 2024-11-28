using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Student_WebApi_ADO_NET.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student_WebApi_ADO_Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController_ADO_Net : ControllerBase
    {
        private ILoggerManager _logger;

        public StudentController_ADO_Net(ILoggerManager logger)
        {
            this._logger = logger;
        }

        [HttpGet("GetStudents_ADO_Net")]
        public async Task<IActionResult> GetStudents_ADO_Net(string UserName = "No Name",
                                                             bool IncludeRelations = true)
        {
            try
            {
                if (false == IncludeRelations)
                {
                    Student Student_Object = new Student();
                    IEnumerable<Student> StudentList = new List<Student>();

                    StudentList = Student_Object.GetData<Student>();

                    List<StudentDto> StudentDtoList;

                    StudentDtoList = StudentList.Adapt<StudentDto[]>().ToList();
                    this._logger.LogInfo($"All Students have been read from GetStudents_ADO_Net action by {UserName}. No Releations Included");
                    return Ok(StudentList);
                }
                else
                {
                    StudentWithAllRelations_ADO_Net StudentWithAllRelations_ADO_Net_Object =
                        new StudentWithAllRelations_ADO_Net();

                    List<StudentWithAllRelations_ADO_Net> StudentWithAllRelations_ADO_Net_List =
                        new List<StudentWithAllRelations_ADO_Net>();
                    StudentWithAllRelations_ADO_Net_List = StudentWithAllRelations_ADO_Net_Object.GetDataWithRelations<StudentWithAllRelations_ADO_Net>(DatabaseCommandStrings.SQLString_SP);
                    this._logger.LogInfo($"All Students have been read from GetStudents_ADO_Net action by {UserName}. Relations Included");
                    return Ok(StudentWithAllRelations_ADO_Net_List);
                }
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetStudents_ADO_Net action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetStudent_ADO_Net/{StudentID}")]
        public async Task<IActionResult> GetStudent_ADO_Net(int StudentID,
                                                            string UserName = "No Name",
                                                            bool IncludeRelations = true)
        {
            try
            {
                // Implementer kode her

                this._logger.LogInfo($"Student with StudentID : {StudentID} have been read from GetStudent_ADO_Net action by {UserName}. Relations Included : {IncludeRelations}");
                return Ok(null);
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetStudent_ADO_Net action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GetStudentsInTeam_ADO_Net/{TeamID}")]
        public async Task<IActionResult> GetStudentsInTeam_ADO_Net(int TeamID,
                                                                   string UserName = "No Name",
                                                                   bool IncludeRelations = true)
        {
            try
            {
                // Implementer kode her

                this._logger.LogInfo($"All Students within TeamID : {TeamID} has been read from GetStudentsInTeam_ADO_Net action by {UserName}. Relations Included : {IncludeRelations}");
                return Ok(null);
            }
            catch (Exception Error)
            {
                this._logger.LogError($"Something went wrong inside GetStudentsInTeam_ADO_Net action for {UserName} : {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // POST: api/Student
        [HttpPost("CreateStudent_ADO_Net")]
        public async Task<IActionResult> CreateStudent_ADO_Net([FromBody] StudentForSaveDto StudentForSaveDto_Object,
                                                                string UserName = "No Name")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateStudent_ADO_Net");
                    return BadRequest(ModelState);
                }

                Student Student_Object = new Student();
                Student_Object = StudentForSaveDto_Object.Adapt<Student>();

                // Metode med Getter-Setter
                int SaveResult = Student_Object.Insert();

                if (SaveResult >= 0)
                {
                    return Ok($"Student : {Student_Object.StudentName} oprettet !!!");
                }
                else
                {
                    return BadRequest($"Noget gik galt, da {Student_Object.StudentName} : skulle oprettes !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside CreateStudent_ADO_Net action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // PUT: api/Student/5
        [HttpPut("UpdateStudent_ADO_Net/{StudentID}")]
        public async Task<IActionResult> UpdateStudent_ADO_Net(int StudentID,
                                                               [FromBody] StudentForUpdateDto StudentForUpdateDto_Object,
                                                               string UserName = "No Name")
        {
            try
            {
                if (StudentID != StudentForUpdateDto_Object.StudentID)
                {
                    //_logger.LogError($"StudentID != StudentForUpdateDto_Object.StudentID for {UserName} in action UpdateStudent");
                    return BadRequest($"StudentID != StudentForUpdateDto_Object.StudentID for {UserName} in action UpdateStudent");
                }

                if (!ModelState.IsValid)
                {
                    //_logger.LogError($"ModelState is Invalid for {UserName} in action UpdateStudent");
                    return BadRequest(ModelState);
                }

                Student Student_Object = new Student();
                Student_Object = StudentForUpdateDto_Object.Adapt<Student>();

                int UpdateResult = Student_Object.Update();

                if (UpdateResult >= 0)
                {
                    return Ok($"Student med StudentId: {Student_Object.StudentID} opdateret !!!");
                }
                else
                {
                    return BadRequest($"Noget gik galt, da Student med StudentId: {Student_Object.StudentID} : skulle opdateres !!!");
                }
            }
            catch (Exception Error)
            {
                //_logger.LogError($"Something went wrong inside UpdateStudent action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        // DELETE: api/Student/5
        [HttpDelete("DeleteStudent_ADO_Net/{StudentID}")]
        public async Task<IActionResult> DeleteStudent_ADO_Net(int StudentID,
                                                               string UserName = "No Name")
        {
            try
            {
                // Implementer kode her
                Student Student_Object = new Student();
                Student_Object.StudentID = StudentID;

                int UpdateResult = Student_Object.Delete();
                this._logger.LogInfo($"Student with ID {StudentID} has been deleted in action DeleteStudent_ADO_Net by {UserName}");
                return Ok($"Student with ID {StudentID} has been deleted in action DeleteStudent_ADO_Net by {UserName}");

            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside DeleteStudent_ADO_Net action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }
    }
}
