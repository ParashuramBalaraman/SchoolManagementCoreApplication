/// <summary>
/// This is the controller for handling degree related requests. 
/// It will then interact with the Service Layer to perform the necessary actions.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-11 14:30 NZT</date>
///     <description>Incorporate CheckInUse methods</description>
/// </modified>


using NLog;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementCoreApplication.ModelBOs.Degrees;
using SchoolManagementCoreApplication.Service.Degrees.Interfaces;
using SchoolManagementCoreApplication.Service.StudentDegrees.Interfaces;
using SchoolManagementCoreApplication.ViewModels.Degree;

namespace SchoolManagementCoreApplication.Areas.Degrees.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDegreeService _degreeService;
        private readonly IStudentDegreeService _studentDegreeService;

        public DegreeController(IDegreeService degreeService, IStudentDegreeService studentDegreeService)
        {
            _degreeService = degreeService;
            _studentDegreeService = studentDegreeService;
        }

        /// <summary>
        /// This is the default get method for the degree controller
        /// </summary>
        /// <returns>Message confirming success</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Info("Get method called");
                return Ok("This is the degree controller");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting a degree by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Degree</returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Info($"Get method called with id: {id}");
                var degree = _degreeService.GetDegree(id);
                if (degree == null)
                {
                    return NotFound("There is no such degree in the system.");
                }
                var students = _studentDegreeService.GetStudentsByDegree(id);
                var teachers = _degreeService.GetTeachersByDegree(id);
                List<string> studentNames = new List<string>();
                List<string> teacherNames = new List<string>();
                foreach (var student in students)
                {
                    studentNames.Add(student.FirstName + " " + student.LastName);
                }
                foreach (var teacher in teachers)
                {
                    teacherNames.Add(teacher.FirstName + " " + teacher.LastName);
                }

                var viewModel = new DegreeDetailsViewModel
                {
                    Degree = degree,
                    StudentNames = studentNames,
                    TeacherNames = teacherNames
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
        /// This is the get method for getting all degrees depending on the active status
        /// </summary>
        /// <returns>List of degrees</returns>
        [HttpGet]
        [Route("Degrees/{active}")]
        public async Task<IActionResult> GetDegrees(bool active)
        {
            try
            {
                _logger.Info($"GetDegrees method called with active: {active}");
                var degrees = _degreeService.GetDegrees(active);
                return Ok(degrees);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetDegrees method with active: {active}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for Upserting a degree
        /// </summary>
        /// <param name="degree"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertDegree([FromBody] DegreeBO degree)
        {
            try
            {
                if (degree == null)
                {
                    _logger.Warn("UpsertDegree method called with null degree");
                    return BadRequest("Degree cannot be null");
                }
                int id = _degreeService.UpsertDegree(degree);
                if (id == 0)
                {
                    _logger.Warn("Degree could not be upserted");
                    return BadRequest("Degree could not be upserted");
                }
                _logger.Info($"Degree successfully upserted with id: {id}");
                return Ok("Degree successfully upserted with id " + id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpsertDegree method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the delete method for deleting a degree
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteDegree(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Warn("DeleteDegree method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                bool success = _degreeService.DeleteDegree(id);
                if (!success)
                {
                    _logger.Warn($"Degree with id {id} could not be deleted");
                    return BadRequest("Degree could not be deleted");
                }
                _logger.Info($"Degree with id {id} successfully deleted");
                return Ok("Degree successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in DeleteDegree method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for toggling the active status of a degree
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
                bool success = _degreeService.ToggleActive(id);
                if (!success)
                {
                    _logger.Warn($"Degree's active status with id {id} could not be changed");
                    return BadRequest("Degree's active status could not be changed");
                }
                _logger.Info($"Degree's active status with id {id} successfully changed");
                return Ok("Degree's active status successfully changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in ToggleActive method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting all teachers by degree id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of teachers</returns>
        [HttpGet]
        [Route("Teachers/{id}")]
        public IActionResult GetTeachersByDegree(int id)
        {
            try
            {
                _logger.Info($"GetTeachersByDegree method called with id: {id}");
                var teachers = _degreeService.GetTeachersByDegree(id);
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetTeachersByDegree method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        ///  This is the get method for checking if a degree is being used
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if degree is in use</returns>
        [HttpGet]
        [Route("Used/{id}")]
        public IActionResult CheckInUse(int id)
        {
            return Ok(_degreeService.CheckInUse(id));
        }
    }
}
