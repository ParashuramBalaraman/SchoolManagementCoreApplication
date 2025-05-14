/// <summary>
/// This is the service for handling student-teacher related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-13</created>
/// <modified>
///     <date>2025-03-25 12:47 NZT</date>
///     <description>Incorporate Query Expressions</description>
/// </modified>

using SchoolManagementCoreApplication.ModelBOs.Student;
using SchoolManagementCoreApplication.ModelBOs.Teacher;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.StudentTeachers.Interfaces;

namespace SchoolManagementCoreApplication.Service.StudentTeachers
{
    public class StudentTeacherService : IStudentTeacherService
    {
        private readonly SchoolDatabaseContext _context;

        public StudentTeacherService(SchoolDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves degrees by student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>List of TeacherBO representing teachers associated with the student.</returns>
        public IEnumerable<TeacherBO> GetTeachersByStudent(int studentId)
        {
            try
            {
                // Gets Student Teachers by Student
                IEnumerable<StudentTeacher> studentTeachers = _context.StudentTeachers.Where(x => x.StudentId == studentId).ToList();
                List<TeacherBO> teacherBOs = new List<TeacherBO>();
                // Parse through each student teacher and get the teacher
                foreach (StudentTeacher studentTeacher in studentTeachers)
                {
                    Teacher teacher = (from t in _context.Teachers
                                       where t.Id == studentTeacher.TeacherId && t.Active == true
                                       select t).FirstOrDefault();

                    if (teacher != null)
                    {
                        // Add the teacher to the list
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
                            Active = teacher.Active,
                            EthnicityId = teacher.EthnicityId,
                            DateOfBirth = teacher.DateOfBirth
                        });
                    }
                }
                return teacherBOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves students by teacher ID.
        /// </summary>
        /// <param name="teacherId">The ID of the teacher</param>
        /// <returns>List of StudentBO representing students associated with the teacher.</returns>
        public IEnumerable<StudentBO> GetStudentsByTeacher(int teacherId)
        {
            try
            {
                // Gets Student Teachers by Teacher
                IEnumerable<StudentTeacher> studentTeachers = _context.StudentTeachers.Where(x => x.TeacherId == teacherId).ToList();
                List<StudentBO> studentBOs = new List<StudentBO>();
                // Parse through each student teacher and get the student
                foreach (StudentTeacher studentTeacher in studentTeachers)
                {
                    Student student = (from s in _context.Students
                                       where s.Id == studentTeacher.StudentId && s.Active == true
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
                            Active = student.Active,
                            EthnicityId = student.EthnicityId,
                            DateOfBirth = student.DateOfBirth
                        });
                    }
                }
                return studentBOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if a student-teacher association exists.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="teacherId"></param>
        /// <returns>Boolean confirming whether an association exists</returns>
        public bool ExistingStudentTeacher(int studentId, int teacherId)
        {
            try
            {
                return _context.StudentTeachers.Where(x => x.StudentId == studentId && x.TeacherId == teacherId).Any();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a student-teacher association.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="teacherId">The ID of the teacher.</param>
        public void CreateStudentTeacher(int studentId, int teacherId)
        {
            try
            {
                // Check if the student and teacher exist
                Student student = _context.Students.Where(x => x.Id == studentId).FirstOrDefault();
                Teacher teacher = _context.Teachers.Where(x => x.Id == teacherId).FirstOrDefault();
                if (student == null || teacher == null)
                {
                    return;
                }
                // Check if the student teacher relationship already exists
                StudentTeacher studentTeacher = (from s in _context.StudentTeachers
                                                 where s.StudentId == studentId && s.TeacherId == teacherId
                                                 select s).FirstOrDefault();
                if (studentTeacher != null)
                {
                    return;
                }
                // Create the student teacher relationship
                _context.StudentTeachers.Add(new StudentTeacher
                {
                    StudentId = studentId,
                    TeacherId = teacherId
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Deletes all student-teacher associations for a given student.
        /// </summary>
        /// <param name="studentId">The ID of the student</param>
        public void DeleteStudentTeachersByStudent(int studentId)
        {
            try
            {
                // Get all student teachers by student
                IEnumerable<StudentTeacher> studentTeachers = _context.StudentTeachers.Where(x => x.StudentId == studentId).ToList();
                // Delete each student teacher relationship
                foreach (StudentTeacher studentTeacher in studentTeachers)
                {
                    _context.StudentTeachers.Remove(studentTeacher);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Deletes a specific student-teacher associations.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="teacherId"></param>
        public void DeleteStudentTeacher(int studentId, int teacherId)
        {
            try
            {
                var studentTeacher = (from s in _context.StudentTeachers
                                      where s.TeacherId == teacherId && s.StudentId == studentId
                                      select s).FirstOrDefault();
                if (studentTeacher != null)
                {
                    _context.StudentTeachers.Remove(studentTeacher);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
