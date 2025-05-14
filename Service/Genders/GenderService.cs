/// <summary>
/// This is the service for handling gender related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-16 13:30 NZT</date>
///     <description>Incorporate Counts methods</description>
/// </modified>

using SchoolManagementCoreApplication.ModelBOs.Degrees;
using SchoolManagementCoreApplication.ModelBOs.Genders;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.Genders.Interfaces;

namespace SchoolManagementCoreApplication.Service.Genders
{
    public class GenderService : IGenderService
    {
        private readonly SchoolDatabaseContext _context;

        public GenderService(SchoolDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method will get a Gender by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>GenderBO</returns>
        public GenderBO GetGender(int id)
        {
            try
            {
                Gender gender = _context.Genders.Find(id);
                if (gender == null)
                {
                    return null;
                }
                GenderBO genderBO = new GenderBO
                {
                    Id = gender.Id,
                    Name = gender.Name,
                    Active = gender.Active
                };
                return genderBO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will get all genders depending on the active status
        /// </summary>
        /// <param name="active"></param>
        /// <returns>List of GenderBOs</returns>
        public List<GenderBO> GetGenders(bool active)
        {
            try
            {
                List<Gender> genders = _context.Genders.Where(s => s.Active == active).ToList();
                List<GenderBO> genderBOs = new List<GenderBO>();
                foreach (Gender gender in genders)
                {
                    GenderBO genderBO = new GenderBO
                    {
                        Id = gender.Id,
                        Name = gender.Name,
                        Active = gender.Active
                    };
                    genderBOs.Add(genderBO);
                }
                return genderBOs;
            }
            catch (Exception ex)
            {
                return new List<GenderBO>();
            }
        }

        /// <summary>
        /// This method will upsert a gender
        /// </summary>
        /// <param name="genderBO"></param>
        /// <returns>Gender Id or 0</returns>
        public int UpsertGender(GenderBO genderBO)
        {
            try
            {
                if (genderBO == null)
                {
                    return 0;
                }
                if (genderBO.Id == 0)
                {
                    Gender gender = new Gender
                    {
                        Id = genderBO.Id,
                        Name = genderBO.Name,
                        Active = genderBO.Active
                    };
                    _context.Genders.Add(gender);
                    _context.SaveChanges();
                    return gender.Id;
                }
                else
                {
                    Gender gender = _context.Genders.Find(genderBO.Id);
                    if (gender == null)
                    {
                        return 0;
                    }
                    gender.Name = genderBO.Name;
                    gender.Active = genderBO.Active;

                    _context.Genders.Update(gender);
                    _context.SaveChanges();
                    return gender.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// This method will delete a gender
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool DeleteGender(int id)
        {
            try
            {
                Gender gender = _context.Genders.Find(id);
                if (gender == null)
                {
                    return false;
                }
                _context.Genders.Remove(gender);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
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
                Gender gender = _context.Genders.Find(id);
                if (gender == null)
                {
                    return false;
                }
                gender.Active = gender.Active == true ? false : true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get count of students by gender
        /// </summary>
        /// <returns>List of tuples with gender name and count</returns>
        public IEnumerable<Tuple<string, int>> GetStudentCounts()
        {
            try
            {
                var genders = _context.Genders.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                foreach (Gender gender in genders)
                {
                    int studentCount = _context.Students.Count(s => s.GenderId == gender.Id && s.Active == true);
                    if (studentCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(gender.Name, studentCount);
                        counts.Add(tuple);
                    }
                }
                return counts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get count of teachers by gender
        /// </summary>
        /// <returns>List of tuples with gender name and count</returns>
        public IEnumerable<Tuple<string, int>> GetTeacherCounts()
        {
            try
            {
                var genders = _context.Genders.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                foreach (Gender gender in genders)
                {
                    int teacherCount = _context.Teachers.Count(s => s.GenderId == gender.Id && s.Active == true);
                    if (teacherCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(gender.Name, teacherCount);
                        counts.Add(tuple);
                    }
                }
                return counts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will get the percentages for students by gender
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with gender name and the corresponding percentage of students</returns>
        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count)
        {
            try
            {
                var genders = _context.Genders.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Gender gender in genders)
                {
                    float studentCount = _context.Students.Count(s => s.GenderId == gender.Id && s.Active == true);
                    if (studentCount != 0)
                    {
                        float percentage = (studentCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(gender.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't stated their gender", difference);
                    percentages.Add(tuple);
                }
                return percentages;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will get the percentages for teachers by gender
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with gender name and the corresponding percentage of teachers</returns>
        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count)
        {
            try
            {
                var genders = _context.Genders.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Gender gender in genders)
                {
                    float teacherCount = _context.Teachers.Count(s => s.GenderId == gender.Id && s.Active == true);
                    if (teacherCount != 0)
                    {
                        float percentage = (teacherCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(gender.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't stated their gender", difference);
                    percentages.Add(tuple);
                }
                return percentages;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if Gender is being used by a student or teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if its used or not</returns>
        public bool CheckInUse(int id)
        {
            var timesUsed = _context.Students.Count(s => s.GenderId == id && s.Active == true) +
                _context.Teachers.Count(s => s.GenderId == id && s.Active == true);
            if (timesUsed > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
