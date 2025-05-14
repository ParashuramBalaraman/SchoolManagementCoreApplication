using SchoolManagementCoreApplication.ModelBOs.Teacher;
using SchoolManagementCoreApplication.ModelBOs.Student;

namespace SchoolManagementCoreApplication.Service.StudentTeachers.Interfaces
{
    public interface IStudentTeacherService
    {
        IEnumerable<TeacherBO> GetTeachersByStudent(int studentId);
        IEnumerable<StudentBO> GetStudentsByTeacher(int teacherId);
        void CreateStudentTeacher(int studentId, int teacherId);
        void DeleteStudentTeachersByStudent(int studentId);
        void DeleteStudentTeacher(int studentId, int teacherId);
        bool ExistingStudentTeacher(int studentId, int teacherId);
    }
}
