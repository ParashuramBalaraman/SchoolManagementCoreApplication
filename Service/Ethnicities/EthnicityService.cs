/// <summary>
/// This is the service for handling ethnicity related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-16 13:46 NZT</date>
///     <description>Incorporate Counts methods</description>
/// </modified>

using NLog;
using SchoolManagementCoreApplication.ModelBOs.Ethnicities;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.Ethnicities.Interfaces;

namespace SchoolManagementCoreApplication.Service.Ethnicities
{
    public class EthnicityService : IEthnicityService
    {
        private readonly SchoolDatabaseContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EthnicityService(SchoolDatabaseContext context)
        {
            _context = context;
        }


        /// <summary>
        /// This method will get a Ethnicity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EthnicityBO</returns>
        public EthnicityBO GetEthnicity(int id)
        {
            try
            {
                _logger.Info("GetEthnicity method called");
                Ethnicity ethnicity = _context.Ethnicities.Find(id);
                if (ethnicity == null)
                {
                    return null;
                }
                EthnicityBO ethnicityBO = new EthnicityBO
                {
                    Id = ethnicity.Id,
                    Name = ethnicity.Name,
                    Active = ethnicity.Active
                };
                return ethnicityBO;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetEthnicity method");
                return null;
            }
        }

        /// <summary>
        /// This method will get all ethnicities depending on the active status
        /// </summary>
        /// <param name="active"></param>
        /// <returns>List of EthnicityBOs</returns>
        public List<EthnicityBO> GetEthnicities(bool active)
        {
            try
            {
                _logger.Info("GetEthnicities method called");
                List<Ethnicity> ethnicities = _context.Ethnicities.Where(s => s.Active == active).ToList();
                List<EthnicityBO> ethnicityBOs = new List<EthnicityBO>();
                foreach (Ethnicity ethnicity in ethnicities)
                {
                    EthnicityBO ethnicityBO = new EthnicityBO
                    {
                        Id = ethnicity.Id,
                        Name = ethnicity.Name,
                        Active = ethnicity.Active
                    };
                    ethnicityBOs.Add(ethnicityBO);
                }
                return ethnicityBOs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetEthnicities method");
                return new List<EthnicityBO>();
            }
        }

        /// <summary>
        /// This method will upsert a ethnicity
        /// </summary>
        /// <param name="ethnicityBO"></param>
        /// <returns>Ethnicity Id or 0</returns>
        public int UpsertEthnicity(EthnicityBO ethnicityBO)
        {
            try
            {
                _logger.Info("UpsertEthnicity method called");
                if (ethnicityBO == null)
                {
                    return 0;
                }
                if (ethnicityBO.Id == 0)
                {
                    Ethnicity ethnicity = new Ethnicity
                    {
                        Id = ethnicityBO.Id,
                        Name = ethnicityBO.Name,
                        Active = ethnicityBO.Active
                    };
                    _context.Ethnicities.Add(ethnicity);
                    _context.SaveChanges();
                    return ethnicity.Id;
                }
                else
                {
                    Ethnicity ethnicity = _context.Ethnicities.Find(ethnicityBO.Id);
                    if (ethnicity == null)
                    {
                        return 0;
                    }
                    ethnicity.Name = ethnicityBO.Name;
                    ethnicity.Active = ethnicityBO.Active;

                    _context.Ethnicities.Update(ethnicity);
                    _context.SaveChanges();
                    return ethnicity.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UpsertEthnicity method");
                return 0;
            }
        }

        /// <summary>
        /// This method will delete a ethnicity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool DeleteEthnicity(int id)
        {
            try
            {
                _logger.Info("DeleteEthnicity method called");
                Ethnicity ethnicity = _context.Ethnicities.Find(id);
                if (ethnicity == null)
                {
                    return false;
                }
                _context.Ethnicities.Remove(ethnicity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in DeleteEthnicity method");
                return false;
            }
        }

        /// <summary>
        /// This method will toggle the active status
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool ToggleActive(int id)
        {
            try
            {
                _logger.Info("ToggleActive method called");
                Ethnicity ethnicity = _context.Ethnicities.Find(id);
                if (ethnicity == null)
                {
                    return false;
                }
                ethnicity.Active = ethnicity.Active == true ? false : true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in ToggleActive method");
                return false;
            }
        }

        /// <summary>
        /// Gets count of students by ethnicity
        /// </summary>
        /// <returns>List of tuples with ethnicity name and count</returns>
        public IEnumerable<Tuple<string, int>> GetStudentCounts()
        {
            try
            {
                _logger.Info("GetStudentCounts method called");
                var ethnicities = _context.Ethnicities.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                foreach (Ethnicity ethnicity in ethnicities)
                {
                    int studentCount = _context.Students.Count(s => s.EthnicityId == ethnicity.Id && s.Active == true);
                    if (studentCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(ethnicity.Name, studentCount);
                        counts.Add(tuple);
                    }
                }
                return counts;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetStudentCounts method");
                return null;
            }
        }

        /// <summary>
        /// Gets count of teachers by ethnicity
        /// </summary>
        /// <returns>List of tuples with ethnicity name and count</returns>
        public IEnumerable<Tuple<string, int>> GetTeacherCounts()
        {
            try
            {
                _logger.Info("GetTeacherCounts method called");
                var ethnicities = _context.Ethnicities.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                foreach (Ethnicity ethnicity in ethnicities)
                {
                    int teacherCount = _context.Teachers.Count(s => s.EthnicityId == ethnicity.Id && s.Active == true);
                    if (teacherCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(ethnicity.Name, teacherCount);
                        counts.Add(tuple);
                    }
                }
                return counts;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetTeacherCounts method");
                return null;
            }
        }

        /// <summary>
        /// This method will get the percentages for students by ethnicity
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with ethnicity name and the corresponding percentage of students</returns>
        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count)
        {
            try
            {
                _logger.Info("GetStudentPercentages method called");
                var ethnicities = _context.Ethnicities.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Ethnicity ethnicity in ethnicities)
                {
                    float studentCount = _context.Students.Count(s => s.EthnicityId == ethnicity.Id && s.Active == true);
                    if (studentCount != 0)
                    {
                        float percentage = (studentCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(ethnicity.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't stated their ethnicity", difference);
                    percentages.Add(tuple);
                }
                return percentages;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetStudentPercentages method");
                return null;
            }
        }

        /// <summary>
        /// This method will get the percentages for teachers by ethnicity
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with ethnicity name and the corresponding percentage of teachers</returns>
        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count)
        {
            try
            {
                _logger.Info("GetTeacherPercentages method called");
                var ethnicities = _context.Ethnicities.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Ethnicity ethnicity in ethnicities)
                {
                    float teacherCount = _context.Teachers.Count(s => s.EthnicityId == ethnicity.Id && s.Active == true);
                    if (teacherCount != 0)
                    {
                        float percentage = (teacherCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(ethnicity.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't stated their ethnicity", difference);
                    percentages.Add(tuple);
                }
                return percentages;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetTeacherPercentages method");
                return null;
            }
        }

        /// <summary>
        /// Checks if Ethnicity is being used by a student or teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if its used or not</returns>
        public bool CheckInUse(int id)
        {
            try
            {
                _logger.Info("CheckInUse method called");
                var timesUsed = _context.Students.Count(s => s.EthnicityId == id && s.Active == true) +
                    _context.Teachers.Count(s => s.EthnicityId == id && s.Active == true);
                if (timesUsed > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in CheckInUse method");
                return false;
            }
        }
    }
}
