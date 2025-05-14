using SchoolManagementCoreApplication.ModelBOs.Degrees;
using SchoolManagementCoreApplication.ModelBOs.Student;
using SchoolManagementCoreApplication.ModelBOs.StudentDegree;

namespace SchoolManagementCoreApplication.Service.StudentDegrees.Interfaces
{
    public interface IStudentDegreeService
    {
        IEnumerable<StudentDegreeBO> GetActiveStudentDegrees();
        IEnumerable<DegreeBO> GetDegreesByStudent(int studentId);
        IEnumerable<StudentBO> GetStudentsByDegree(int degreeId);
        void CreateStudentDegree(int studentId, int degreeId);
        void DeleteStudentDegreesByStudent(int studentId);
        void DeleteStudentDegree(int studentId, int degreeId);
        bool ExistingStudentDegree(int studentId, int degreeId);
    }
}
