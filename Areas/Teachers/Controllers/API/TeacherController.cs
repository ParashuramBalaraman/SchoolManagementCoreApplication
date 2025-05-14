/// <summary>
/// This is the controller for handling teacher related requests. 
/// It will then interact with the Service Layer to perform the necessary actions.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-11 13:45 NZT</date>
///     <description>Incorporate CheckInUse method</description>
/// </modified>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SchoolManagementCoreApplication.ModelBOs.Teacher;
using SchoolManagementCoreApplication.Service.StudentTeachers.Interfaces;
using SchoolManagementCoreApplication.Service.Teachers.Interfaces;
using SchoolManagementCoreApplication.ViewModels.Teacher;

namespace SchoolManagementCoreApplication.Areas.Teachers.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ITeacherService _teacherService;
        private readonly IStudentTeacherService _studentTeacherService;

        public TeacherController(ITeacherService teacherService, IStudentTeacherService studentTeacherService)
        {
            _teacherService = teacherService;
            _studentTeacherService = studentTeacherService;
        }

        /// <summary>
        /// This is the default get method for the teacher controller
        /// </summary>
        /// <returns>Message confirming success</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Info("Get method called");
                return Ok("This is the teacher controller");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting a teacher by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Teacher</returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Info($"Get method called with id: {id}");
                var teacher = _teacherService.GetTeacher(id);
                if (teacher == null)
                {
                    return NotFound("There is no such teacher in the system.");
                }
                var students = _studentTeacherService.GetStudentsByTeacher(id);
                List<string> studentNames = new List<string>();
                foreach (var student in students)
                {
                    // Concatenate the first name and last name of the student then add it to the list.
                    studentNames.Add(student.FirstName + " " + student.LastName);
                }
                var viewModel = new TeacherDetailsViewModel
                {
                    Teacher = teacher,
                    StudentNames = studentNames
                };
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in Get method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting all teachers depending on the active status
        /// </summary>
        /// <returns>List of teachers</returns>
        [HttpGet]
        [Route("Teachers/{active}")]
        public async Task<IActionResult> GetTeachers(bool active)
        {
            try
            {
                _logger.Info($"GetTeachers method called with active: {active}");
                var teachers = _teacherService.GetTeachers(active);
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetTeachers method with active: {active}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for Upserting a teacher
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertTeacher([FromBody] TeacherBO teacher)
        {
            try
            {
                if (teacher == null)
                {
                    _logger.Warn("UpsertTeacher method called with null teacher");
                    return BadRequest("Teacher cannot be null");
                }
                int id = _teacherService.UpsertTeacher(teacher);
                if (id == 0)
                {
                    _logger.Warn("Teacher could not be upserted");
                    return BadRequest("Teacher could not be upserted");
                }
                _logger.Info($"Teacher successfully upserted with id: {id}");
                return Ok("Teacher successfully upserted with id " + id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpsertTeacher method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the delete method for deleting a teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Warn("DeleteTeacher method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                bool success = _teacherService.DeleteTeacher(id);
                if (!success)
                {
                    _logger.Warn($"Teacher with id {id} could not be deleted");
                    return BadRequest("Teacher could not be deleted");
                }
                _logger.Info($"Teacher with id {id} successfully deleted");
                return Ok("Teacher successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in DeleteTeacher method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for toggling the active status of a teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpPost]
        [Route("Active/{id}")]
        public IActionResult ToggleActive(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Warn("ToggleActive method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                bool success = _teacherService.ToggleActive(id);
                if (!success)
                {
                    _logger.Warn($"Teacher's active status with id {id} could not be changed");
                    return BadRequest("Teacher's active status could not be changed");
                }
                _logger.Info($"Teacher's active status with id {id} successfully changed");
                return Ok("Teacher's active status successfully changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in ToggleActive method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        ///  This is the get method for checking if a teacher is being used
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if teacher is in use</returns>
        [HttpGet]
        [Route("Used/{id}")]
        public IActionResult CheckInUse(int id)
        {
            return Ok(_teacherService.CheckInUse(id));
        }
    }
}
