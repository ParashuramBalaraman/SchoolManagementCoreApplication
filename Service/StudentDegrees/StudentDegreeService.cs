/// <summary>
/// This is the service for handling student-degree related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-13</created>
/// <modified>
///     <date>2025-03-25 13:35 NZT</date>
///     <description>Incorporate Try/Catch block in each method</description>
/// </modified>

using SchoolManagementCoreApplication.ModelBOs.Degrees;
using SchoolManagementCoreApplication.ModelBOs.Student;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.StudentDegrees.Interfaces;
using SchoolManagementCoreApplication.ModelBOs.StudentDegree;
using NLog;

namespace SchoolManagementCoreApplication.Service.StudentDegrees
{
    public class StudentDegreeService : IStudentDegreeService
    {
        private readonly SchoolDatabaseContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public StudentDegreeService(SchoolDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<StudentDegreeBO> GetActiveStudentDegrees()
        {
            try
            {
                _logger.Info("GetActiveStudentDegrees method called");
                var studentDegrees = _context.StudentDegrees.ToList();
                var activeStudentDegrees = new List<StudentDegreeBO>();
                foreach (var studentDegree in studentDegrees)
                {
                    var student = _context.Students.FirstOrDefault(s => s.Id == studentDegree.StudentId && s.Active);
                    if (student != null)
                    {
                        activeStudentDegrees.Add(new StudentDegreeBO
                        {
                            Id = studentDegree.Id,
                            StudentId = studentDegree.StudentId,
                            DegreeId = studentDegree.DegreeId,
                        });
                    }
                }
                return activeStudentDegrees;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetActiveStudentDegrees method");
                return null;
            }
        }

        /// <summary>
        /// Retrieves degrees by student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>List of DegreeBO representing degrees associated with the student.</returns>
        public IEnumerable<DegreeBO> GetDegreesByStudent(int studentId)
        {
            try
            {
                _logger.Info("GetDegreesByStudent method called");
                // Gets Student Degrees by Student
                IEnumerable<StudentDegree> studentDegrees = _context.StudentDegrees.Where(x => x.StudentId == studentId).ToList();
                List<DegreeBO> degreeBOs = new List<DegreeBO>();
                // Parse through each student degree and get the degree
                foreach (StudentDegree studentDegree in studentDegrees)
                {
                    Degree degree = (from d in _context.Degrees
                                     where d.Id == studentDegree.DegreeId && d.Active == true
                                     select d).FirstOrDefault();
                    if (degree != null)
                    {
                        // Add the degree to the list
                        degreeBOs.Add(new DegreeBO
                        {
                            Id = degree.Id,
                            Name = degree.Name,
                            DepartmentId = degree.DepartmentId,
                            Active = degree.Active
                        });
                    }
                }
                return degreeBOs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetDegreesByStudent method");
                return null;
            }
        }

        /// <summary>
        /// Retrieves students by degree ID.
        /// </summary>
        /// <param name="degreeId">The ID of the degree.</param>
        /// <returns>List of StudentBO representing students associated with the degree.</returns>
        public IEnumerable<StudentBO> GetStudentsByDegree(int degreeId)
        {
            try
            {
                _logger.Info("GetStudentsByDegree method called");
                // Gets Student Degrees by Degree
                IEnumerable<StudentDegree> studentDegrees = _context.StudentDegrees.Where(x => x.DegreeId == degreeId).ToList();
                List<StudentBO> studentBOs = new List<StudentBO>();
                // Parse through each student degree and get the student
                foreach (StudentDegree studentDegree in studentDegrees)
                {
                    Student student = (from s in _context.Students
                                       where s.Id == studentDegree.StudentId && s.Active == true
                                       select s).FirstOrDefault();
                    if (student != null)
                    {
                        // Add the student to the list
                        studentBOs.Add(new StudentBO
                        {
                            Id = student.Id,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            MiddleName = student.MiddleName,
                            PreferredName = student.PreferredName,
                            Email = student.Email,
                            Phone = student.Phone,
                            Address = student.Address,
                            GenderId = student.GenderId,
                            EthnicityId = student.EthnicityId,
                            DateOfBirth = student.DateOfBirth,
                            Active = student.Active
                        });
                    }
                }
                return studentBOs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetStudentsByDegree method");
                return null;
            }
        }

        /// <summary>
        /// Checks if a student-degree association exists.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="degreeId"></param>
        /// <returns>Boolean confirming whether an association exists</returns>
        public bool ExistingStudentDegree(int studentId, int degreeId)
        {
            try
            {
                _logger.Info("ExistingStudentDegree method called");
                return _context.StudentDegrees.Where(x => x.StudentId == studentId && x.DegreeId == degreeId).Any();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in ExistingStudentDegree method");
                return false;
            }
        }

        /// <summary>
        /// Creates a student-degree association.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="degreeId">The ID of the degree.</param>
        public void CreateStudentDegree(int studentId, int degreeId)
        {
            try
            {
                _logger.Info("CreateStudentDegree method called");
                var existingStudentDegree = _context.StudentDegrees.Where(x => x.StudentId == studentId && x.DegreeId == degreeId).FirstOrDefault();
                // If the student-degree association already exists, do nothing
                if (existingStudentDegree != null)
                {
                    return;
                }
                // Otherwise, create the student-degree association
                _context.StudentDegrees.Add(new StudentDegree
                {
                    StudentId = studentId,
                    DegreeId = degreeId
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in CreateStudentDegree method");
            }
        }

        /// <summary>
        /// Deletes all student-degree associations for a given student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        public void DeleteStudentDegreesByStudent(int studentId)
        {
            try
            {
                _logger.Info("DeleteStudentDegreesByStudent method called");
                // Get all student degrees for the student
                IEnumerable<StudentDegree> studentDegrees = _context.StudentDegrees.Where(x => x.StudentId == studentId).ToList();
                foreach (StudentDegree studentDegree in studentDegrees)
                {
                    // Remove each student degree
                    _context.StudentDegrees.Remove(studentDegree);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in DeleteStudentDegreesByStudent method");
            }
        }

        /// <summary>
        /// Deletes a specific student-degree association.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="degreeId"></param>
        public void DeleteStudentDegree(int studentId, int degreeId)
        {
            try
            {
                _logger.Info("DeleteStudentDegree method called");
                var studentDegree = _context.StudentDegrees.Where(x => x.StudentId == studentId && x.DegreeId == degreeId).FirstOrDefault();
                if (studentDegree != null)
                {
                    _context.StudentDegrees.Remove(studentDegree);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in DeleteStudentDegree method");
            }
        }
    }
}
