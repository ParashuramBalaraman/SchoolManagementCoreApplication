/// <summary>
/// This is the service for handling teacher related actions. 
/// It will then interact with the DAL to perform the necessary actions and return the result to the Controller.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-12</created>
/// <modified>
///     <date>2025-04-11 13:30 NZT</date>
///     <description>Implement CheckInUse method</description>
/// </modified>

using SchoolManagementCoreApplication.ModelBOs.Teacher;
using SchoolManagementCoreApplication.Models;
using SchoolManagementCoreApplication.Service.Teachers.Interfaces;

namespace SchoolManagementCoreApplication.Service.Teachers
{
    public class TeacherService : ITeacherService
    {
        private readonly SchoolDatabaseContext _context;

        public TeacherService(SchoolDatabaseContext context)
        {
            _context = context;
        }

        public TeacherBO GetTeacher(int id)
        {
            try
            {
                Teacher teacher = _context.Teachers.Find(id);
                if (teacher == null)
                {
                    return null;
                }
                TeacherBO teacherBO = new TeacherBO
                {
                    Id = teacher.Id,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    MiddleName = teacher.MiddleName,
                    PreferredName = teacher.PreferredName,
                    DateOfBirth = teacher.DateOfBirth,
                    Email = teacher.Email,
                    Phone = teacher.Phone,
                    Address = teacher.Address,
                    GenderId = teacher.GenderId,
                    EthnicityId = teacher.EthnicityId,
                    DegreeId = teacher.DegreeId,
                    Active = teacher.Active
                };
                return teacherBO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TeacherBO> GetTeachers(bool active)
        {
            try
            {
                List<Teacher> teachers = _context.Teachers.Where(s => s.Active == active).ToList();
                List<TeacherBO> teacherBOs = new List<TeacherBO>();
                foreach (Teacher teacher in teachers)
                {
                    TeacherBO teacherBO = new TeacherBO
                    {
                        Id = teacher.Id,
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName,
                        MiddleName = teacher.MiddleName,
                        PreferredName = teacher.PreferredName,
                        DateOfBirth = teacher.DateOfBirth,
                        Email = teacher.Email,
                        Phone = teacher.Phone,
                        Address = teacher.Address,
                        GenderId = teacher.GenderId,
                        EthnicityId = teacher.EthnicityId,
                        DegreeId = teacher.DegreeId,
                        Active = teacher.Active
                    };
                    teacherBOs.Add(teacherBO);
                }
                return teacherBOs;
            }
            catch (Exception ex)
            {
                return new List<TeacherBO>();
            }
        }

        public int UpsertTeacher(TeacherBO teacherBO)
        {
            try
            {
                if (teacherBO == null)
                {
                    return 0;
                }
                if (teacherBO.Id == 0)
                {
                    Teacher teacher = new Teacher
                    {
                        FirstName = teacherBO.FirstName,
                        LastName = teacherBO.LastName,
                        MiddleName = teacherBO.MiddleName,
                        PreferredName = teacherBO.PreferredName,
                        DateOfBirth = teacherBO.DateOfBirth,
                        Email = teacherBO.Email,
                        Phone = teacherBO.Phone,
                        Address = teacherBO.Address,
                        GenderId = teacherBO.GenderId,
                        EthnicityId = teacherBO.EthnicityId,
                        DegreeId = teacherBO.DegreeId,
                        Active = teacherBO.Active
                    };
                    _context.Teachers.Add(teacher);
                    _context.SaveChanges();
                    return teacher.Id;
                }
                else
                {
                    Teacher teacher = _context.Teachers.Find(teacherBO.Id);
                    if (teacher == null)
                    {
                        return 0;
                    }
                    teacher.FirstName = teacherBO.FirstName;
                    teacher.LastName = teacherBO.LastName;
                    teacher.MiddleName = teacherBO.MiddleName;
                    teacher.PreferredName = teacherBO.PreferredName;
                    teacher.DateOfBirth = teacherBO.DateOfBirth;
                    teacher.Email = teacherBO.Email;
                    teacher.Phone = teacherBO.Phone;
                    teacher.Address = teacherBO.Address;
                    teacher.GenderId = teacherBO.GenderId;
                    teacher.EthnicityId = teacherBO.EthnicityId;
                    teacher.DegreeId = teacherBO.DegreeId;
                    teacher.Active = teacherBO.Active;

                    _context.Teachers.Update(teacher);
                    _context.SaveChanges();
                    return teacher.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DeleteTeacher(int id)
        {
            try
            {
                Teacher teacher = _context.Teachers.Find(id);
                if (teacher == null)
                {
                    return false;
                }
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ToggleActive(int id)
        {
            try
            {
                Teacher teacher = _context.Teachers.Find(id);
                if (teacher == null)
                {
                    return false;
                }
                teacher.Active = teacher.Active == true ? false : true;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if Teacher is being used by a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean depicting if its used or not</returns>
        public bool CheckInUse(int id)
        {
            var studentTeachers = _context.StudentTeachers.Where(st => st.TeacherId == id).ToList();
            var timesUsed = 0;
            foreach (var studentTeacher in studentTeachers)
            {
                var student = _context.Students.Where(x => x.Id == studentTeacher.StudentId && x.Active == true);
                if (student != null)
                {
                    timesUsed++;
                }
            }

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
