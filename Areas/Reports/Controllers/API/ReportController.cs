/// <summary>
/// This is the controller for handling report related requests. 
/// It will then interact with the Service Layer to perform the necessary actions.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-04-08</created>
/// <modified>
///     <date>2025-04-08 15:50 NZT</date>
///     <description>Incorporate GetEthnicityPercentagesByTeachers</description>
/// </modified>


using Microsoft.AspNetCore.Mvc;
using NLog;
using SchoolManagementCoreApplication.Service.Degrees.Interfaces;
using SchoolManagementCoreApplication.Service.Departments.Interfaces;
using SchoolManagementCoreApplication.Service.Ethnicities.Interfaces;
using SchoolManagementCoreApplication.Service.Genders.Interfaces;
using SchoolManagementCoreApplication.Service.StudentDegrees.Interfaces;
using SchoolManagementCoreApplication.Service.Students.Interfaces;
using SchoolManagementCoreApplication.Service.Teachers.Interfaces;

namespace SchoolManagementCoreApplication.Areas.Reports.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IGenderService _genderService;
        private readonly IDegreeService _degreeService;
        private readonly IEthnicityService _ethnicityService;
        private readonly IDepartmentService _departmentService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IStudentDegreeService _studentDegreeService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ReportController(IGenderService genderService, IDegreeService degreeService, IEthnicityService ethnicityService, IDepartmentService departmentService, IStudentService studentService, ITeacherService teacherService, IStudentDegreeService studentDegreeService)
        {
            _genderService = genderService;
            _degreeService = degreeService;
            _ethnicityService = ethnicityService;
            _departmentService = departmentService;
            _studentService = studentService;
            _teacherService = teacherService;
            _studentDegreeService = studentDegreeService;
        }

        /// <summary>
        /// Gets percentages of students by gender
        /// </summary>
        /// <returns>List of tuples with gender name and percentage</returns>
        [HttpGet]
        [Route("Students/Genders")]
        public async Task<IActionResult> GetGenderPercentagesByStudents()
        {
            try
            {
                _logger.Info("GetGenderPercentagesByStudents method called");
                int studentCount = _studentService.GetStudents(true).Count();
                var genderCounts = _genderService.GetStudentPercentages(studentCount);
                return Ok(genderCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetGenderPercentagesByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets counts of students by gender
        /// </summary>
        /// <returns>List of tuples with gender name and count</returns>
        [HttpGet]
        [Route("Students/Genders/Count")]
        public async Task<IActionResult> GetGenderCountsByStudents()
        {
            try
            {
                _logger.Info("GetGenderCountsByStudents method called");
                var genderCounts = _genderService.GetStudentCounts();
                return Ok(genderCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetGenderCountsByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of teachers by gender
        /// </summary>
        /// <returns>List of tuples with gender name and percentage</returns>
        [HttpGet]
        [Route("Teachers/Genders")]
        public async Task<IActionResult> GetGenderPercentagesByTeachers()
        {
            try
            {
                _logger.Info("GetGenderPercentagesByTeachers method called");
                int teacherCount = _teacherService.GetTeachers(true).Count();
                var genderCounts = _genderService.GetTeacherPercentages(teacherCount);
                return Ok(genderCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetGenderPercentagesByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets counts of teachers by gender
        /// </summary>
        /// <returns>List of tuples with gender name and count</returns>
        [HttpGet]
        [Route("Teachers/Genders/Count")]
        public async Task<IActionResult> GetGenderCountsByTeachers()
        {
            try
            {
                _logger.Info("GetGenderCountsByTeachers method called");
                var genderCounts = _genderService.GetTeacherCounts();
                return Ok(genderCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetGenderCountsByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of students by degree
        /// </summary>
        /// <returns>List of tuples with degree name and percentage</returns>
        [HttpGet]
        [Route("Students/Degrees")]
        public async Task<IActionResult> GetDegreePercentagesByStudents()
        {
            try
            {
                _logger.Info("GetDegreePercentagesByStudents method called");
                int studentDegreeCount = _studentDegreeService.GetActiveStudentDegrees().Count();
                var degreeCounts = _degreeService.GetStudentPercentages(studentDegreeCount);
                return Ok(degreeCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDegreePercentagesByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets count of students by degree
        /// </summary>
        /// <returns>List of tuples with degree name and count</returns>
        [HttpGet]
        [Route("Students/Degrees/Count")]
        public async Task<IActionResult> GetDegreeCountsByStudents()
        {
            try
            {
                _logger.Info("GetDegreeCountsByStudents method called");
                int studentDegreeCount = _studentDegreeService.GetActiveStudentDegrees().Count();
                var degreeCounts = _degreeService.GetStudentCounts(studentDegreeCount);
                return Ok(degreeCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDegreeCountsByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of teachers by degree
        /// </summary>
        /// <returns>List of tuples with degree name and percentage</returns>
        [HttpGet]
        [Route("Teachers/Degrees")]
        public async Task<IActionResult> GetDegreePercentagesByTeachers()
        {
            try
            {
                _logger.Info("GetDegreePercentagesByTeachers method called");
                int teacherCount = _teacherService.GetTeachers(true).Count();
                var degreeCounts = _degreeService.GetTeacherPercentages(teacherCount);
                return Ok(degreeCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDegreePercentagesByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets count of teachers by degree
        /// </summary>
        /// <returns>List of tuples with degree name and count</returns>
        [HttpGet]
        [Route("Teachers/Degrees/Count")]
        public async Task<IActionResult> GetDegreeCountsByTeachers()
        {
            try
            {
                _logger.Info("GetDegreeCountsByTeachers method called");
                var degreeCounts = _degreeService.GetTeacherCounts();
                return Ok(degreeCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDegreeCountsByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of students by department
        /// </summary>
        /// <returns>List of tuples with department name and percentage</returns>
        [HttpGet]
        [Route("Students/Departments")]
        public async Task<IActionResult> GetDepartmentPercentagesByStudents()
        {
            try
            {
                _logger.Info("GetDepartmentPercentagesByStudents method called");
                int studentCount = _studentDegreeService.GetActiveStudentDegrees().Count();
                var departmentCounts = _departmentService.GetStudentPercentages(studentCount);
                return Ok(departmentCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDepartmentPercentagesByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets count of students by department
        /// </summary>
        /// <returns>List of tuples with department name and count</returns>
        [HttpGet]
        [Route("Students/Departments/Count")]
        public async Task<IActionResult> GetDepartmentCountsByStudents()
        {
            try
            {
                _logger.Info("GetDepartmentCountsByStudents method called");
                var departmentCounts = _departmentService.GetStudentCounts();
                return Ok(departmentCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDepartmentCountsByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of teachers by department
        /// </summary>
        /// <returns>List of tuples with department name and percentage</returns>
        [HttpGet]
        [Route("Teachers/Departments")]
        public async Task<IActionResult> GetDepartmentPercentagesByTeachers()
        {
            try
            {
                _logger.Info("GetDepartmentPercentagesByTeachers method called");
                int teacherCount = _teacherService.GetTeachers(true).Count();
                var departmentCounts= _departmentService.GetTeacherPercentages(teacherCount);
                return Ok(departmentCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDepartmentPercentagesByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets count of teachers by department
        /// </summary>
        /// <returns>List of tuples with department name and count</returns>
        [HttpGet]
        [Route("Teachers/Departments/Count")]
        public async Task<IActionResult> GetDepartmentCountsByTeachers()
        {
            try
            {
                _logger.Info("GetDepartmentCountsByTeachers method called");
                var departmentCounts = _departmentService.GetTeacherCounts();
                return Ok(departmentCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDepartmentCountsByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of degrees by department
        /// </summary>
        /// <returns>List of tuples with department name and percentage</returns>
        [HttpGet]
        [Route("Degrees/Departments")]
        public async Task<IActionResult> GetDepartmentPercentagesByDegrees()
        {
            try
            {
                _logger.Info("GetDepartmentPercentagesByDegrees method called");
                int degreeCount = _degreeService.GetDegrees(true).Count();
                var departmentCounts = _departmentService.GetDegreePercentages(degreeCount);
                return Ok(departmentCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDepartmentPercentagesByDegrees method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets count of degrees by department
        /// </summary>
        /// <returns>List of tuples with department name and count</returns>
        [HttpGet]
        [Route("Degrees/Departments/Count")]
        public async Task<IActionResult> GetDepartmentCountsByDegrees()
        {
            try
            {
                _logger.Info("GetDepartmentCountsByDegrees method called");
                var departmentCounts = _departmentService.GetDegreeCounts();
                return Ok(departmentCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDepartmentCountsByDegrees method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of students by ethnicity
        /// </summary>
        /// <returns>List of tuples with ethnicity name and percentage</returns>
        [HttpGet]
        [Route("Students/Ethnicities")]
        public async Task<IActionResult> GetEthnicityPercentagesByStudents()
        {
            try
            {
                _logger.Info("GetEthnicityPercentagesByStudents method called");
                int studentCount = _studentService.GetStudents(true).Count();
                var ethnicityCounts = _ethnicityService.GetStudentPercentages(studentCount);
                return Ok(ethnicityCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetEthnicityPercentagesByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets count of students by ethnicity
        /// </summary>
        /// <returns>List of tuples with ethnicity name and count</returns>
        [HttpGet]
        [Route("Students/Ethnicities/Count")]
        public async Task<IActionResult> GetEthnicityCountsByStudents()
        {
            try
            {
                _logger.Info("GetEthnicityCountsByStudents method called");
                var ethnicityCounts = _ethnicityService.GetStudentCounts();
                return Ok(ethnicityCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetEthnicityCountsByStudents method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets percentages of teachers by ethnicity
        /// </summary>
        /// <returns>List of tuples with ethnicity name and percentage</returns>
        [HttpGet]
        [Route("Teachers/Ethnicities")]
        public async Task<IActionResult> GetEthnicityPercentagesByTeachers()
        {
            try
            {
                _logger.Info("GetEthnicityPercentagesByTeachers method called");
                int teacherCount = _teacherService.GetTeachers(true).Count();
                var ethnicityCounts = _ethnicityService.GetTeacherPercentages(teacherCount);
                return Ok(ethnicityCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetEthnicityPercentagesByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets counts of teachers by ethnicity
        /// </summary>
        /// <returns>List of tuples with ethnicity name and count</returns>
        [HttpGet]
        [Route("Teachers/Ethnicities/Count")]
        public async Task<IActionResult> GetEthnicityCountsByTeachers()
        {
            try
            {
                _logger.Info("GetEthnicityCountsByTeachers method called");
                var ethnicityCounts = _ethnicityService.GetTeacherCounts();
                return Ok(ethnicityCounts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetEthnicityCountsByTeachers method");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
