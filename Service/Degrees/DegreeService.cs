/// <summary>
/// This is the service for handling degree related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-16 13:10 NZT</date>
///     <description>Incorporate Counts methods</description>
/// </modified>

using SchoolManagementCoreApplication.ModelBOs.Degrees;
using SchoolManagementCoreApplication.ModelBOs.Teacher;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.Degrees.Interfaces;

namespace SchoolManagementCoreApplication.Service.Degrees
{
    public class DegreeService : IDegreeService
    {
        private readonly SchoolDatabaseContext _context;

        public DegreeService(SchoolDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method will get a degree by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DegreeBO</returns>
        //Change to ActionsResult later with the error/success messages
        public DegreeBO GetDegree(int id)
        {
            try
            {
                Degree degree = _context.Degrees.Find(id);
                if (degree == null)
                {
                    return null;
                }
                DegreeBO degreeBO = new DegreeBO
                {
                    Id = degree.Id,
                    Name = degree.Name,
                    DepartmentId = degree.DepartmentId,
                    Active = degree.Active
                };
                return degreeBO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will get all degrees depending on the active status
        /// </summary>
        /// <param name="active"></param>
        /// <returns>List of DegreeBOs</returns>
        public List<DegreeBO> GetDegrees(bool active)
        {
            try
            {
                List<Degree> degrees = _context.Degrees.Where(s => s.Active == active).ToList();
                List<DegreeBO> degreeBOs = new List<DegreeBO>();
                foreach (Degree degree in degrees)
                {
                    DegreeBO degreeBO = new DegreeBO
                    {
                        Id = degree.Id,
                        Name = degree.Name,
                        DepartmentId = degree.DepartmentId,
                        Active = degree.Active
                    };
                    degreeBOs.Add(degreeBO);
                }
                return degreeBOs;
            }
            catch (Exception ex)
            {
                return new List<DegreeBO>();
            }
        }

        /// <summary>
        /// This method will upsert a degree
        /// </summary>
        /// <param name="degreeBO"></param>
        /// <returns>Degree Id or 0</returns>
        public int UpsertDegree(DegreeBO degreeBO)
        {
            try
            {
                if (degreeBO == null)
                {
                    return 0;
                }
                if (degreeBO.Id == 0)
                {
                    Degree degree = new Degree
                    {
                        Id = degreeBO.Id,
                        Name = degreeBO.Name,
                        DepartmentId = degreeBO.DepartmentId,
                        Active = degreeBO.Active
                    };
                    _context.Degrees.Add(degree);
                    _context.SaveChanges();
                    return degree.Id;
                }
                else
                {
                    Degree degree = _context.Degrees.Find(degreeBO.Id);
                    if (degree == null)
                    {
                        return 0;
                    }
                    degree.Name = degreeBO.Name;
                    degree.DepartmentId = degreeBO.DepartmentId;
                    degree.Active = degreeBO.Active;

                    _context.Degrees.Update(degree);
                    _context.SaveChanges();
                    return degree.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// This method will delete a degree
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool DeleteDegree(int id)
        {
            try
            {
                Degree degree = _context.Degrees.Find(id);
                if (degree == null)
                {
                    return false;
                }
                _context.Degrees.Remove(degree);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method will toggle the active status of a degree
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool ToggleActive(int id)
        {
            try
            {
                Degree degree = _context.Degrees.Find(id);
                if (degree == null)
                {
                    return false;
                }
                degree.Active = degree.Active == true ? false : true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method will get all teachers by degree
        /// </summary>
        /// <param name="degreeId"></param>
        /// <returns>List of TeacherBOs</returns>
        public IEnumerable<TeacherBO> GetTeachersByDegree(int degreeId)
        {
            try
            {
                var teachers = _context.Teachers.Where(x => x.DegreeId == degreeId && x.Active == true).ToList();
                List<TeacherBO> teacherBOs = new List<TeacherBO>();
                foreach (var teacher in teachers)
                {
                    teacherBOs.Add(new TeacherBO
                    {
                        Id = teacher.Id,
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName,
                        MiddleName = teacher.MiddleName,
                        PreferredName = teacher.PreferredName,
                        DegreeId = teacher.DegreeId,
                        Email = teacher.Email,
                        Phone = teacher.Phone,
                        Address = teacher.Address,
                        GenderId = teacher.GenderId,
                        EthnicityId = teacher.EthnicityId,
                        DateOfBirth = teacher.DateOfBirth
                    });
                }
                return teacherBOs;
            }
            catch (Exception ex)
            {
                return new List<TeacherBO>();
            }
        }

        /// <summary>
        /// This method will get the student counts by degree
        /// </summary>
        /// <returns>List of tuples with degree name and the corresponding count of students</returns>
        public IEnumerable<Tuple<string, int>> GetStudentCounts(int count)
        {
            try
            {
                var degrees = _context.Degrees.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                int totalCount = 0;
                foreach (Degree degree in degrees)
                {
                    var studentDegrees = _context.StudentDegrees.Where(x => x.DegreeId == degree.Id).ToList();
                    int studentCount = 0;
                    foreach (var studentDegree in studentDegrees)
                    {
                        var student = _context.Students.Find(studentDegree.StudentId);
                        if (student != null && student.Active == true)
                        {
                            studentCount += 1;
                        }
                    }
                    totalCount += studentCount;
                    if (studentCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(degree.Name, studentCount);
                        counts.Add(tuple);
                    }
                }
                if (totalCount < count)
                {
                    int difference = count - totalCount;
                    Tuple<string, int> tuple = new Tuple<string, int>("Hasn't Registered For Degree", difference);
                    counts.Add(tuple);
                }
                return counts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will get the teacher counts by degree
        /// </summary>
        /// <returns>List of tuples with degree name and the corresponding count of teachers</returns>
        public IEnumerable<Tuple<string, float>> GetTeacherCounts()
        {
            try
            {
                var degrees = _context.Degrees.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, float>>();
                foreach (Degree degree in degrees)
                {
                    float teacherCount = _context.Teachers.Count(s => s.DegreeId == degree.Id && s.Active == true);
                    if (teacherCount != 0)
                    {
                        Tuple<string, float> tuple = new Tuple<string, float>(degree.Name, teacherCount);
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
        /// This method will get the percentages for students by degree
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with degree name and the corresponding percentage of students</returns>
        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count)
        {
            try
            {
                var degrees = _context.Degrees.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple< string, float>>();
                float percentCount = 0;
                foreach (Degree degree in degrees)
                {
                    var studentDegrees = _context.StudentDegrees.Where(x => x.DegreeId == degree.Id).ToList();
                    float studentCount = 0;
                    foreach (var studentDegree in studentDegrees)
                    {
                        var student = _context.Students.Find(studentDegree.StudentId);
                        if (student != null && student.Active == true)
                        {
                            studentCount += 1;
                        }
                    }
                    if (studentCount != 0)
                    {
                        float percentage = (studentCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(degree.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't Registered For Degree", difference);
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
        /// This method will get the percentages for teachers by degree
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with degree name and the corresponding percentage of teachers</returns>
        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count)
        {
            try
            {
                var degrees = _context.Degrees.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Degree degree in degrees)
                {
                    float teacherCount = _context.Teachers.Count(s => s.DegreeId == degree.Id && s.Active == true);
                    if (teacherCount != 0)
                    {
                        float percentage = (teacherCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(degree.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't Registered For Degree", difference);
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
        /// Checks if Degree is being used by a student or teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if its used or not</returns>
        public bool CheckInUse(int id)
        {
            var studentDegrees = _context.StudentDegrees.Where(sd => sd.DegreeId == id).ToList();
            var timesUsed = 0;
            foreach (var studentDegree in studentDegrees)
            {
                var student = _context.Students.Where(x => x.Id == studentDegree.StudentId && x.Active == true);
                if (student != null)
                {
                    timesUsed++;
                }
            }

            timesUsed += _context.Teachers.Count(s => s.DegreeId == id && s.Active == true);

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