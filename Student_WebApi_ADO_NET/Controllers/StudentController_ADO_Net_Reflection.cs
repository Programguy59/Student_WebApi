using Microsoft.AspNetCore.Mvc;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Mapster;
using System.Net;
using Student_WebApi_ADO_NET.ViewModels;

namespace Student_WebApi_ADO_NET.Controllers
{
    public class StudentController_ADO_Net_Reflection : Controller
    {
        private ILoggerManager _logger;

        public StudentController_ADO_Net_Reflection(ILoggerManager logger)
        {
            this._logger = logger;
        }

        // POST: api/Student
        [HttpPost("CreateStudent_ADO_Net_Reflection")]
        public async Task<IActionResult> CreateStudent_ADO_Net_Reflection([FromBody] StudentForSaveDto StudentForSaveDto_Object,
                                                                           string UserName = "No Name")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateStudent_ADO_Net_Reflection");
                    return BadRequest(ModelState);
                }

                Student Student_Object = new Student();
                Student_Object = StudentForSaveDto_Object.Adapt<Student>();

                // Fuld Generisk metode
                int SaveResult = Student_Object.InsertObjectToDatabase<Student>(Student.TABLE_NAME);

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
                _logger.LogError($"Something went wrong inside CreateStudent_ADO_Net_Reflection action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }


        [HttpPut("UpdateStudent_ADO_Net_Reflection/{StudentId}")]
        public async Task<IActionResult> UpdateStudent_ADO_Net_Reflection(int StudentId,
                                                                    [FromBody] StudentForUpdateDto studentForUpdateDto,
                                                                    string UserName = "No Name")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"ModelState is invalid for {UserName} in action UpdateStudent_ADO_Net_Reflection.");
                    return BadRequest(ModelState);
                }

                // Map the DTO to the Student object and set the ID
                Student studentObject = studentForUpdateDto.Adapt<Student>();
                studentObject.StudentID = StudentId;

                // Call the generic method to update the object in the database
                int updateResult = studentObject.UpdateObjectInDatabase<Student>(Student.TABLE_NAME);

                if (updateResult >= 0)
                {
                    return Ok($"Student with ID {StudentId} updated successfully.");
                }
                else
                {
                    return BadRequest($"An error occurred while updating the student with ID {StudentId}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in UpdateStudent_ADO_Net_Reflection action for {UserName}: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error: {ex}");
            }
        }


        [HttpDelete("DeleteStudent_ADO_Net_Reflection")]
        public async Task<IActionResult> DeleteStudent_ADO_Net_Reflection(int StudentId,
                                                                           string UserName = "No Name")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateStudent_ADO_Net_Reflection");
                    return BadRequest(ModelState);
                }

                Student Student_Object = new Student();
                Student_Object.StudentID = StudentId;

                // Fuld Generisk metode
                int SaveResult = Student_Object.DeleteObjectToDatabase<Student>(Student.TABLE_NAME);

                if (SaveResult >= 0)
                {
                    return Ok($"Student : {Student_Object.StudentName} Deletet !!!");
                }
                else
                {
                    return BadRequest($"Noget gik galt, da {Student_Object.StudentName} : skulle DEleted !!!");
                }
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside DeleteStudent_ADO_Net_Reflection action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }
        [HttpGet("GEtStudent_ADO_Net_Reflection")]
        public async Task<IActionResult> GetStudent_ADO_Net_Reflection(int StudentId,
                                                                           string UserName = "No Name")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateStudent_ADO_Net_Reflection");
                    return BadRequest(ModelState);
                }

                Student Student_Object = new Student();
                Student_Object.StudentID=StudentId;

                IEnumerable<Student> Students = new List<Student>();
                // Fuld Generisk metode
                Students = Student_Object.GetObjectToDatabase<Student>(Student.TABLE_NAME);

                return Ok(Students);
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside CreateStudent_ADO_Net_Reflection action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }

        [HttpGet("GEtStudents_ADO_Net_Reflection")]
        public async Task<IActionResult> GetStudents_ADO_Net_Reflection(string UserName = "No Name")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogError($"ModelState is Invalid for {UserName} in action CreateStudent_ADO_Net_Reflection");
                    return BadRequest(ModelState);
                }

                Student Student_Object = new Student();

                IEnumerable<Student> Students = new List<Student>();
                // Fuld Generisk metode
                Students = Student_Object.GetObjectsToDatabase<Student>(Student.TABLE_NAME);

                return Ok(Students);
            }
            catch (Exception Error)
            {
                _logger.LogError($"Something went wrong inside CreateStudent_ADO_Net_Reflection action for {UserName}: {Error.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error : {Error.ToString()}");
            }
        }
    }
}
