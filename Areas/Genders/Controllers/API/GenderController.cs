/// <summary>
/// This is the controller for handling gender related requests. 
/// It will then interact with the Service Layer to perform the necessary actions.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-11 11:53 NZT</date>
///     <description>Incorporate CheckInUse method</description>
/// </modified>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SchoolManagementCoreApplication.ModelBOs.Genders;
using SchoolManagementCoreApplication.Service.Genders.Interfaces;

namespace SchoolManagementCoreApplication.Areas.Genders.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGenderService _genderService;

        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }

        /// <summary>
        /// This is the default get method for the gender controller
        /// </summary>
        /// <returns>Message confirming success</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Info("Get method called");
                return Ok("This is the gender controller");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting a gender by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Gender</returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Info($"Get method called with id: {id}");
                var gender = _genderService.GetGender(id);
                if (gender == null)
                {
                    return NotFound("There is no such gender in the system.");
                }
                return Ok(gender);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in Get method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting all genders depending on the active status
        /// </summary>
        /// <returns>List of genders</returns>
        [HttpGet]
        [Route("Genders/{active}")]
        public async Task<IActionResult> GetGenders(bool active)
        {
            try
            {
                _logger.Info($"GetGenders method called with active: {active}");
                var genders = _genderService.GetGenders(active);
                return Ok(genders);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetGenders method with active: {active}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for Upserting a gender
        /// </summary>
        /// <param name="gender"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertGender([FromBody] GenderBO gender)
        {
            try
            {
                if (gender == null)
                {
                    _logger.Warn("UpsertGender method called with null gender");
                    return BadRequest("Gender cannot be null");
                }
                int id = _genderService.UpsertGender(gender);
                if (id == 0)
                {
                    _logger.Warn("Gender could not be upserted");
                    return BadRequest("Gender could not be upserted");
                }
                _logger.Info($"Gender successfully upserted with id: {id}");
                return Ok("Gender successfully upserted with id " + id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpsertGender method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the delete method for deleting a gender
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteGender(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Warn("DeleteGender method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                bool success = _genderService.DeleteGender(id);
                if (!success)
                {
                    _logger.Warn($"Gender with id {id} could not be deleted");
                    return BadRequest("Gender could not be deleted");
                }
                _logger.Info($"Gender with id {id} successfully deleted");
                return Ok("Gender successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in DeleteGender method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for toggling the active status of a gender
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
                bool success = _genderService.ToggleActive(id);
                if (!success)
                {
                    _logger.Warn($"Gender's active status with id {id} could not be changed");
                    return BadRequest("Gender's active status could not be changed");
                }
                _logger.Info($"Gender's active status with id {id} successfully changed");
                return Ok("Gender's active status successfully changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in ToggleActive method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        ///  This is the get method for checking if a gender is being used
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if gender is in use</returns>
        [HttpGet]
        [Route("Used/{id}")]
        public IActionResult CheckInUse(int id)
        {
            return Ok(_genderService.CheckInUse(id));
        }
    }
}
