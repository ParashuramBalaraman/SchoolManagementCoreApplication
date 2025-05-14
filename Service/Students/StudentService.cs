/// <summary>
/// This is the service for handling student related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-02-14 13:30 NZT</date>
///     <description>Incorporate Try/Catch block in each method</description>
/// </modified>

using SchoolManagementCoreApplication.ModelBOs.Student;
using SchoolManagementCoreApplication.ModelBOs.StudentAllocation;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.Students.Interfaces;
using SchoolManagementCoreApplication.Service.StudentDegrees.Interfaces;
using SchoolManagementCoreApplication.Service.StudentTeachers.Interfaces;
using SchoolManagementCoreApplication.Service.Teachers.Interfaces;
using SchoolManagementCoreApplication.Service.Degrees.Interfaces;

namespace SchoolManagementCoreApplication.Service.Students
{
    public class StudentService : IStudentService
    {
        private readonly SchoolDatabaseContext _context;

        private readonly IStudentTeacherService _studentTeacherService;
        private readonly IStudentDegreeService _studentDegreeService;
        private readonly ITeacherService _teacherService;
        private readonly IDegreeService _degreeService;

        public StudentService(SchoolDatabaseContext context, IStudentDegreeService studentDegreeService,
            IStudentTeacherService studentTeacherService, ITeacherService teacherService, IDegreeService degreeService)
        {
            _context = context;
            _studentDegreeService = studentDegreeService;
            _studentTeacherService = studentTeacherService;
            _teacherService = teacherService;
            _degreeService = degreeService;
        }

        /// <summary>
        /// This method will get a student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>StudentBO</returns>
        public StudentBO GetStudent(int id)
        {
            try
            {
                Student student = _context.Students.Find(id);
                if (student == null)
                {
                    return null;
                }
                StudentBO studentBO = new StudentBO
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    MiddleName = student.MiddleName,
                    PreferredName = student.PreferredName,
                    DateOfBirth = student.DateOfBirth,
                    Email = student.Email,
                    Phone = student.Phone,
                    Address = student.Address,
                    GenderId = student.GenderId,
                    EthnicityId = student.EthnicityId,
                    Active = student.Active
                };
                return studentBO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will get all students depending on the active status
        /// </summary>
        /// <param name="active"></param>
        /// <returns>List of StudentBOs</returns>
        public List<StudentBO> GetStudents(bool active)
        {
            try
            {
                List<Student> students = (from s in _context.Students
                                          where s.Active == active
                                          select s).ToList();
                List<StudentBO> studentBOs = new List<StudentBO>();
                foreach (Student student in students)
                {
                    StudentBO studentBO = new StudentBO
                    {
                        Id = student.Id,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        MiddleName = student.MiddleName,
                        PreferredName = student.PreferredName,
                        DateOfBirth = student.DateOfBirth,
                        Email = student.Email,
                        Phone = student.Phone,
                        Address = student.Address,
                        GenderId = student.GenderId,
                        EthnicityId = student.EthnicityId,
                        Active = student.Active
                    };
                    studentBOs.Add(studentBO);
                }
                return studentBOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will upsert a student
        /// </summary>
        /// <param name="studentBO"></param>
        /// <returns>Student Id or 0</returns>
        public int UpsertStudent(StudentBO studentBO)
        {
            try
            {
                if (studentBO == null)
                {
                    return 0;
                }
                if (studentBO.Id == 0)
                {
                    Student student = new Student
                    {
                        FirstName = studentBO.FirstName,
                        LastName = studentBO.LastName,
                        MiddleName = studentBO.MiddleName,
                        PreferredName = studentBO.PreferredName,
                        DateOfBirth = studentBO.DateOfBirth,
                        Email = studentBO.Email,
                        Phone = studentBO.Phone,
                        Address = studentBO.Address,
                        GenderId = studentBO.GenderId,
                        EthnicityId = studentBO.EthnicityId,
                        Active = studentBO.Active
                    };
                    _context.Students.Add(student);
                    _context.SaveChanges();
                    return student.Id;
                }
                else
                {
                    Student student = _context.Students.Find(studentBO.Id);
                    if (student == null)
                    {
                        return 0;
                    }
                    student.FirstName = studentBO.FirstName;
                    student.LastName = studentBO.LastName;
                    student.MiddleName = studentBO.MiddleName;
                    student.PreferredName = studentBO.PreferredName;
                    student.DateOfBirth = studentBO.DateOfBirth;
                    student.Email = studentBO.Email;
                    student.Phone = studentBO.Phone;
                    student.Address = studentBO.Address;
                    student.GenderId = studentBO.GenderId;
                    student.EthnicityId = studentBO.EthnicityId;
                    student.Active = studentBO.Active;

                    _context.Students.Update(student);
                    _context.SaveChanges();
                    return student.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// This method will delete a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool DeleteStudent(int id)
        {
            try
            {
                Student student = _context.Students.Find(id);
                if (student == null)
                {
                    return false;
                }
                _context.Students.Remove(student);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method will toggle the active status of a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean indicating success</returns>
        public bool ToggleActive(int id)
        {
            try
            {
                Student student = _context.Students.Find(id);
                if (student == null)
                {
                    return false;
                }
                student.Active = student.Active == true ? false : true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method will allocate a degree and teacher to a student
        /// </summary>
        /// <param name="studentAllocationBO"></param>
        /// <returns>Boolean indicating success</returns>
        public bool Allocate(StudentAllocationBO studentAllocationBO)
        {
            try
            {
                var teacher = _teacherService.GetTeacher(studentAllocationBO.TeacherId);
                var degree = _degreeService.GetDegree(studentAllocationBO.DegreeId);
                if (teacher == null || degree == null)
                {
                    return false;
                }
                bool existingStudentDegree = _studentDegreeService.ExistingStudentDegree(studentAllocationBO.StudentId, studentAllocationBO.DegreeId);
                bool existingStudentTeacher = _studentTeacherService.ExistingStudentTeacher(studentAllocationBO.StudentId, studentAllocationBO.TeacherId);
                if (existingStudentDegree)
                {
                    if (existingStudentTeacher)
                    {
                        return false;
                    }
                    var existingStudentAllocaton = GetAllocation(studentAllocationBO.StudentId, studentAllocationBO.DegreeId);
                    DeleteAllocation(existingStudentAllocaton);
                }
                _studentDegreeService.CreateStudentDegree(studentAllocationBO.StudentId, studentAllocationBO.DegreeId);
                _studentTeacherService.CreateStudentTeacher(studentAllocationBO.StudentId, studentAllocationBO.TeacherId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method will delete an allocation
        /// </summary>
        /// <param name="studentAllocationBO"></param>
        /// <returns>Boolean indicating success</returns>
        public bool DeleteAllocation(StudentAllocationBO studentAllocationBO)
        {
            try
            {
                bool existingStudentDegree = _studentDegreeService.ExistingStudentDegree(studentAllocationBO.StudentId, studentAllocationBO.DegreeId);
                bool existingStudentTeacher = _studentTeacherService.ExistingStudentTeacher(studentAllocationBO.StudentId, studentAllocationBO.TeacherId);
                if (!existingStudentDegree || !existingStudentTeacher)
                {
                    return false;
                }
                _studentDegreeService.DeleteStudentDegree(studentAllocationBO.StudentId, studentAllocationBO.DegreeId);
                _studentTeacherService.DeleteStudentTeacher(studentAllocationBO.StudentId, studentAllocationBO.TeacherId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method will get the allocation from a student and degree
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="DegreeId"></param>
        /// <returns>StudentAllocatioBO</returns>
        public StudentAllocationBO GetAllocation(int studentId, int DegreeId)
        {
            var teachers = _context.Teachers.Where(t => t.DegreeId == DegreeId).ToList();
            var studentTeacher = _context.StudentTeachers.Where(st => st.StudentId == studentId).ToList();
            foreach (Teacher teacher in teachers)
            {
                foreach (StudentTeacher st in studentTeacher)
                {
                    if (teacher.Id == st.TeacherId)
                    {
                        return new StudentAllocationBO
                        {
                            StudentId = studentId,
                            DegreeId = DegreeId,
                            TeacherId = teacher.Id
                        };
                    }
                }
            }
            return null;
        }
    }
}
