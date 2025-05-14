/// <summary>
/// This is the service for handling department related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-16 13:42 NZT</date>
///     <description>Incorporate Counts methods</description>
/// </modified>

using SchoolManagementCoreApplication.ModelBOs.Departments;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.Departments.Interfaces;

namespace SchoolManagementCoreApplication.Service.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly SchoolDatabaseContext _context;

        public DepartmentService(SchoolDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method will get a Department by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>DepartmentBO</returns>
        public DepartmentBO GetDepartment(int id)
        {
            try
            {
                Department department = _context.Departments.Find(id);
                if (department == null)
                {
                    return null;
                }
                DepartmentBO departmentBO = new DepartmentBO
                {
                    Id = department.Id,
                    Name = department.Name,
                    Active = department.Active
                };
                return departmentBO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will get all departments depending on the active status
        /// </summary>
        /// <param name="active"></param>
        /// <returns>List of DepartmentBOs</returns>
        public List<DepartmentBO> GetDepartments(bool active)
        {
            try
            {
                List<Department> departments = _context.Departments.Where(s => s.Active == active).ToList();
                List<DepartmentBO> departmentBOs = new List<DepartmentBO>();
                foreach (Department department in departments)
                {
                    DepartmentBO departmentBO = new DepartmentBO
                    {
                        Id = department.Id,
                        Name = department.Name,
                        Active = department.Active
                    };
                    departmentBOs.Add(departmentBO);
                }
                return departmentBOs;
            }
            catch (Exception ex)
            {
                return new List<DepartmentBO>();
            }
        }

        /// <summary>
        /// This method will upsert a department
        /// </summary>
        /// <param name="departmentBO"></param>
        /// <returns>Department Id or 0</returns>
        public int UpsertDepartment(DepartmentBO departmentBO)
        {
            try
            {
                if (departmentBO == null)
                {
                    return 0;
                }
                if (departmentBO.Id == 0)
                {
                    Department department = new Department
                    {
                        Id = departmentBO.Id,
                        Name = departmentBO.Name,
                        Active = departmentBO.Active
                    };
                    _context.Departments.Add(department);
                    _context.SaveChanges();
                    return department.Id;
                }
                else
                {
                    Department department = _context.Departments.Find(departmentBO.Id);
                    if (department == null)
                    {
                        return 0;
                    }
                    department.Name = departmentBO.Name;
                    department.Active = departmentBO.Active;

                    _context.Departments.Update(department);
                    _context.SaveChanges();
                    return department.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// This method will delete a department
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool DeleteDepartment(int id)
        {
            try
            {
                Department department = _context.Departments.Find(id);
                if (department == null)
                {
                    return false;
                }
                _context.Departments.Remove(department);
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
                Department department = _context.Departments.Find(id);
                if (department == null)
                {
                    return false;
                }
                department.Active = department.Active == true ? false : true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets count of students by department
        /// </summary>
        /// <returns>List of tuples with department name and count</returns>
        public IEnumerable<Tuple<string, int>> GetStudentCounts()
        {
            try
            {
                var departments = _context.Departments.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                foreach (Department department in departments)
                {
                    var degrees = _context.Degrees.Where(d => d.DepartmentId == department.Id && d.Active == true).ToList();
                    int studentCount = 0;
                    foreach (Degree degree in degrees)
                    {
                        var studentDegrees = _context.StudentDegrees.Where(s => s.DegreeId == degree.Id).ToList();
                        foreach (StudentDegree studentDegree in studentDegrees)
                        {
                            var student = _context.Students.FirstOrDefault(s => s.Id == studentDegree.StudentId && s.Active);
                            if (student != null)
                            {
                                studentCount++;
                            }
                        }
                    }
                    if (studentCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(department.Name, studentCount);
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
        /// Gets count of teachers by department
        /// </summary>
        /// <returns>List of tuples with department name and count</returns>
        public IEnumerable<Tuple<string, int>> GetTeacherCounts()
        {
            try
            {
                var departments = _context.Departments.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                foreach (Department department in departments)
                {
                    var degrees = _context.Degrees.Where(d => d.DepartmentId == department.Id && d.Active == true).ToList();
                    int teacherCount = 0;
                    foreach (Degree degree in degrees)
                    {
                        teacherCount += _context.Teachers.Count(s => s.DegreeId == degree.Id && s.Active == true);
                    }
                    if (teacherCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(department.Name, teacherCount);
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
        /// Gets count of degrees by department
        /// </summary>
        /// <returns>List of tuples with department name and count</returns>
        public IEnumerable<Tuple<string, int>> GetDegreeCounts()
        {
            try
            {
                var departments = _context.Departments.Where(x => x.Active == true).ToList();
                var counts = new List<Tuple<string, int>>();
                foreach (Department department in departments)
                {
                    var degreeCount = _context.Degrees.Count(d => d.DepartmentId == department.Id && d.Active == true);
                    if (degreeCount != 0)
                    {
                        Tuple<string, int> tuple = new Tuple<string, int>(department.Name, degreeCount);
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
        /// This method will get the percentages for students by department
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with department name and the corresponding percentage of students</returns>
        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count)
        {
            try
            {
                var departments = _context.Departments.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Department department in departments)
                {
                    var degrees = _context.Degrees.Where(d => d.DepartmentId == department.Id && d.Active == true).ToList();
                    float studentCount = 0;
                    foreach (Degree degree in degrees)
                    {
                        var studentDegrees = _context.StudentDegrees.Where(s => s.DegreeId == degree.Id).ToList();
                        foreach (StudentDegree studentDegree in studentDegrees)
                        {
                            var student = _context.Students.FirstOrDefault(s => s.Id == studentDegree.StudentId && s.Active);
                            if (student != null)
                            {
                                studentCount++;
                            }
                        }
                    }
                    if (studentCount != 0)
                    {
                        float percentage = (studentCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(department.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't Been Assigned to Department", difference);
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
        /// This method will get the percentages for teachers by department
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with department name and the corresponding percentage of teachers</returns>
        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count)
        {
            try
            {
                var departments = _context.Departments.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Department department in departments)
                {
                    var degrees = _context.Degrees.Where(d => d.DepartmentId == department.Id && d.Active == true).ToList();
                    float teacherCount = 0;
                    foreach (Degree degree in degrees)
                    {
                        teacherCount += _context.Teachers.Count(s => s.DegreeId == degree.Id && s.Active == true);
                    }
                    if (teacherCount != 0)
                    {
                        float percentage = (teacherCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(department.Name, percentage);
                        percentages.Add(tuple);
                    }
                }        
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't Been Assigned to Department", difference);
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
        /// This method will get the percentages for degrees by department
        /// </summary>
        /// <param name="count"></param>
        /// <returns>List of tuples with department name and the corresponding percentage of degrees</returns>
        public IEnumerable<Tuple<string, float>> GetDegreePercentages(float count)
        {
            try
            {
                var departments = _context.Departments.Where(x => x.Active == true).ToList();
                var percentages = new List<Tuple<string, float>>();
                float percentCount = 0;
                foreach (Department department in departments)
                {
                    var degreeCount = _context.Degrees.Count(d => d.DepartmentId == department.Id && d.Active == true);
                    if (degreeCount != 0)
                    {
                        float percentage = (degreeCount * 100) / count;
                        percentCount += percentage;
                        Tuple<string, float> tuple = new Tuple<string, float>(department.Name, percentage);
                        percentages.Add(tuple);
                    }
                }
                if (percentCount < 100)
                {
                    float difference = 100 - percentCount;
                    Tuple<string, float> tuple = new Tuple<string, float>("Hasn't Been Assigned to Department", difference);
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
        /// This method will check if a department is in use by a degree
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if department is in use</returns>
        public bool CheckInUse(int id)
        {
            var timesUsed = _context.Degrees.Count(s => s.DepartmentId == id && s.Active == true);
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
