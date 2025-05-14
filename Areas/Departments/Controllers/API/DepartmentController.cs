/// <summary>
/// This is the controller for handling department related requests. 
/// It will then interact with the Service Layer to perform the necessary actions.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-11 12:00 NZT</date>
///     <description>Incorporate CheckInUse method</description>
/// </modified>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SchoolManagementCoreApplication.ModelBOs.Departments;
using SchoolManagementCoreApplication.Service.Departments.Interfaces;

namespace SchoolManagementCoreApplication.Areas.Departments.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// This is the default get method for the department controller
        /// </summary>
        /// <returns>Message confirming success</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Info("Get method called");
                return Ok("This is the department controller");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting a department by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Department</returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Info($"Get method called with id: {id}");
                var department = _departmentService.GetDepartment(id);
                if (department == null)
                {
                    return NotFound("There is no such department in the system.");
                }
                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in Get method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting all departments depending on the active status
        /// </summary>
        /// <returns>List of departments</returns>
        [HttpGet]
        [Route("Departments/{active}")]
        public async Task<IActionResult> GetDepartments(bool active)
        {
            try
            {
                _logger.Info($"GetDepartments method called with active: {active}");
                var departments = _departmentService.GetDepartments(active);
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetDepartments method with active: {active}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for Upserting a department
        /// </summary>
        /// <param name="department"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertDepartment([FromBody] DepartmentBO department)
        {
            try
            {
                if (department == null)
                {
                    _logger.Warn("UpsertDepartment method called with null department");
                    return BadRequest("Department cannot be null");
                }
                int id = _departmentService.UpsertDepartment(department);
                if (id == 0)
                {
                    _logger.Warn("Department could not be upserted");
                    return BadRequest("Department could not be upserted");
                }
                _logger.Info($"Department successfully upserted with id: {id}");
                return Ok("Department successfully upserted with id " + id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpsertDepartment method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the delete method for deleting a department
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Warn("DeleteDepartment method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                bool success = _departmentService.DeleteDepartment(id);
                if (!success)
                {
                    _logger.Warn($"Department with id {id} could not be deleted");
                    return BadRequest("Department could not be deleted");
                }
                _logger.Info($"Department with id {id} successfully deleted");
                return Ok("Department successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in DeleteDepartment method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for toggling the active status of a department
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
                bool success = _departmentService.ToggleActive(id);
                if (!success)
                {
                    _logger.Warn($"Department's active status with id {id} could not be changed");
                    return BadRequest("Department's active status could not be changed");
                }
                _logger.Info($"Department's active status with id {id} successfully changed");
                return Ok("Department's active status successfully changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in ToggleActive method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        ///  This is the get method for checking if a department is being used
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if department is in use</returns>
        [HttpGet]
        [Route("Used/{id}")]
        public IActionResult CheckInUse(int id)
        {
            return Ok(_departmentService.CheckInUse(id));
        }
    }
}
