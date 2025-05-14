/// <summary>
/// This is the controller for handling ethnicity related requests. 
/// It will then interact with the Service Layer to perform the necessary actions.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-11 12:07 NZT</date>
///     <description>Incorporate CheckInUse method</description>
/// </modified>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SchoolManagementCoreApplication.ModelBOs.Ethnicities;
using SchoolManagementCoreApplication.Service.Ethnicities.Interfaces;

namespace SchoolManagementCoreApplication.Areas.Ethnicities.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EthnicityController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IEthnicityService _ethnicityService;

        public EthnicityController(IEthnicityService ethnicityService)
        {
            _ethnicityService = ethnicityService;
        }

        /// <summary>
        /// This is the default get method for the ethnicity controller
        /// </summary>
        /// <returns>Message confirming success</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Info("Get method called");
                return Ok("This is the ethnicity controller");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in Get method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting an ethnicity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ethnicity</returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _logger.Info($"Get method called with id: {id}");
                var ethnicity = _ethnicityService.GetEthnicity(id);
                if (ethnicity == null)
                {
                    return NotFound("There is no such ethnicity in the system.");
                }
                return Ok(ethnicity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in Get method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the get method for getting all ethnicities depending on the active status
        /// </summary>
        /// <returns>List of ethnicities</returns>
        [HttpGet]
        [Route("Ethnicities/{active}")]
        public async Task<IActionResult> GetEthnicities(bool active)
        {
            try
            {
                _logger.Info($"GetEthnicities method called with active: {active}");
                var ethnicities = _ethnicityService.GetEthnicities(active);
                return Ok(ethnicities);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in GetEthnicities method with active: {active}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for Upserting an ethnicity
        /// </summary>
        /// <param name="ethnicity"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertEthnicity([FromBody] EthnicityBO ethnicity)
        {
            try
            {
                if (ethnicity == null)
                {
                    _logger.Warn("UpsertEthnicity method called with null ethnicity");
                    return BadRequest("Ethnicity cannot be null");
                }
                int id = _ethnicityService.UpsertEthnicity(ethnicity);
                if (id == 0)
                {
                    _logger.Warn("Ethnicity could not be upserted");
                    return BadRequest("Ethnicity could not be upserted");
                }
                _logger.Info($"Ethnicity successfully upserted with id: {id}");
                return Ok("Ethnicity successfully upserted with id " + id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpsertEthnicity method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the delete method for deleting an ethnicity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message Confirming Success</returns>
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteEthnicity(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Warn("DeleteEthnicity method called with id 0");
                    return BadRequest("Id cannot be 0");
                }
                bool success = _ethnicityService.DeleteEthnicity(id);
                if (!success)
                {
                    _logger.Warn($"Ethnicity with id {id} could not be deleted");
                    return BadRequest("Ethnicity could not be deleted");
                }
                _logger.Info($"Ethnicity with id {id} successfully deleted");
                return Ok("Ethnicity successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in DeleteEthnicity method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// This is the post method for toggling the active status of an ethnicity
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
                bool success = _ethnicityService.ToggleActive(id);
                if (!success)
                {
                    _logger.Warn($"Ethnicity's active status with id {id} could not be changed");
                    return BadRequest("Ethnicity's active status could not be changed");
                }
                _logger.Info($"Ethnicity's active status with id {id} successfully changed");
                return Ok("Ethnicity's active status successfully changed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error in ToggleActive method with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        ///  This is the get method for checking if a ethnicity is being used
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if ethnicity is in use</returns>
        [HttpGet]
        [Route("Used/{id}")]
        public IActionResult CheckInUse(int id)
        {
            return Ok(_ethnicityService.CheckInUse(id));
        }
    }
}
