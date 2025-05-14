/// <summary>
/// This is the controller for handling student related requests. 
/// It will then interact with the Service Layer to perform the necessary actions.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-03-24 19:04 NZT</date>
///     <description>Incorporate Try/Catch blocks in the methods</description>
/// </modified>

using NLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementCoreApplication.ModelBOs.Student;
using SchoolManagementCoreApplication.Service.StudentDegrees.Interfaces;
using SchoolManagementCoreApplication.Service.Students.Interfaces;
using SchoolManagementCoreApplication.Service.StudentTeachers.Interfaces;
using SchoolManagementCoreApplication.ViewModels.Students;
using SchoolManagementCoreApplication.ModelBOs.StudentAllocation;
using SchoolManagementCoreApplication.Service.Degrees.Interfaces;

namespace SchoolManagementCoreApplication.Areas.Students.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IStudentTeacherService _studentTeacherService;
        private readonly IStudentDegreeService _studentDegreeService;
        private readonly IDegreeService _degreeService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public StudentController(IStudentService studentService, IStudentDegreeService studentDegreeService, IStudentTeacherService studentTeacherService, IDegreeService degreeService)
        {
            _studentService = studentService;
            _studentDegreeService = studentDegreeService;
            _studentTeacherService = studentTeacherService;
            _degreeService = degreeService;
        }

        /// <summary>
        /// This is the default get method for the student controller
        /// </summary>
        /// <returns>Message confirming success</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Info("Get method called");
                return Ok("This is the student controller");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting a student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Student</returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Info($"Get method called with id: {id}");
                var student = _studentService.GetStudent(id);
                if (student == null)
                {
                    return NotFound("There is no such student in the system.");
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in Get method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting all students depending on the active status
        /// </summary>
        /// <returns>List of students</returns>
        [HttpGet]
        [Route("Students/{active}")]
        public async Task<IActionResult> GetStudents(bool active)
        {
            try
            {
                _logger.Info($"GetStudents method called with active: {active}");
                var students = _studentService.GetStudents(active);
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetStudents method with active: {active}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for Upserting a student
        /// </summary>
        /// <param name="student"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertStudent([FromBody] StudentBO student)
        {
            try
            {
                if (student == null)
                {
                    _logger.Warn("UpsertStudent method called with null student");
                    return BadRequest("Student cannot be null");
                }
                int id = _studentService.UpsertStudent(student);
                if (id == 0)
                {
                    _logger.Warn("Student could not be upserted");
                    return BadRequest("Student could not be upserted");
                }
                _logger.Info($"Student successfully upserted with id: {id}");
                return Ok("Student successfully upserted with id " + id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpsertStudent method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the delete method for deleting a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Warn("DeleteStudent method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                bool success = _studentService.DeleteStudent(id);
                if (!success)
                {
                    _logger.Warn($"Student with id {id} could not be deleted");
                    return BadRequest("Student could not be deleted");
                }
                _logger.Info($"Student with id {id} successfully deleted");
                return Ok("Student successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in DeleteStudent method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for toggling the active status of a student
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
                bool success = _studentService.ToggleActive(id);
                if (!success)
                {
                    _logger.Warn($"Student's active status with id {id} could not be changed");
                    return BadRequest("Student's active status could not be changed");
                }
                _logger.Info($"Student's active status with id {id} successfully changed");
                return Ok("Student's active status successfully changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in ToggleActive method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting the details of a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ViewModel with student details</returns>
        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            try
            {
                _logger.Info($"Details method called with id: {id}");
                var student = _studentService.GetStudent(id);
                if (student == null)
                {
                    return NotFound("There is no such student in the system.");
                }
                var teachers = _studentTeacherService.GetTeachersByStudent(id);
                List<string> teacherNames = new List<string>();
                foreach (var teacher in teachers)
                {
                    // Concatenate the first name and last name of the teacher then add it to the list.
                    var degree = _degreeService.GetDegree(teacher.DegreeId);
                    teacherNames.Add(teacher.FirstName + " " + teacher.LastName + " (" + degree.Name + ")");
                }

                var degrees = _studentDegreeService.GetDegreesByStudent(id);
                List<string> degreeNames = new List<string>();
                foreach (var degree in degrees)
                {
                    degreeNames.Add(degree.Name);
                }

                var viewModel = new StudentDetailsViewModel
                {
                    Student = student,
                    TeacherNames = teacherNames,
                    DegreeNames = degreeNames
                };
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in Details method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("Allocate")]
        public IActionResult Allocation([FromBody] StudentAllocationBO studentAllocationBO)
        {
            try
            {
                if (studentAllocationBO.StudentId == 0 || studentAllocationBO.DegreeId == 0 || studentAllocationBO.TeacherId == 0)
                {
                    _logger.Warn("Allocation method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                var result = _studentService.Allocate(studentAllocationBO);
                if (!result)
                {
                    return NotFound("Allocation could not be done.");
                }
                _logger.Info($"Allocation successfully done for studentId: {studentAllocationBO.StudentId}, degreeId: {studentAllocationBO.DegreeId}, teacherId: {studentAllocationBO.TeacherId}");
                return Ok("Allocation successfully done.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Allocation method");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("Allocate")]
        public IActionResult DeleteAllocation([FromBody] int studentId, int degreeId, int teacherId)
        {
            try
            {
                if (studentId == 0 || degreeId == 0 || teacherId == 0)
                {
                    _logger.Warn("DeleteAllocation method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                StudentAllocationBO studentAllocationBO = new StudentAllocationBO
                {
                    StudentId = studentId,
                    DegreeId = degreeId,
                    TeacherId = teacherId
                };
                var result = _studentService.DeleteAllocation(studentAllocationBO);
                if (!result)
                {
                    return NotFound("There is no such allocation in the system.");
                }
                _logger.Info($"Allocation successfully deleted for studentId: {studentId}, degreeId: {degreeId}, teacherId: {teacherId}");
                return Ok("Allocation successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in DeleteAllocation method");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
