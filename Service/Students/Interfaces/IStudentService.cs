using SchoolManagementCoreApplication.ModelBOs.Student;
using SchoolManagementCoreApplication.ModelBOs.StudentAllocation;

namespace SchoolManagementCoreApplication.Service.Students.Interfaces
{
    public interface IStudentService
    {
        public StudentBO GetStudent(int id);

        public List<StudentBO> GetStudents(bool active);

        public int UpsertStudent(StudentBO student);

        public bool DeleteStudent(int id);

        public bool ToggleActive(int id);

        public bool Allocate(StudentAllocationBO studentAllocation);

        public bool DeleteAllocation(StudentAllocationBO studentAllocationBO);

        public StudentAllocationBO GetAllocation(int studentId, int degreeId);
    }
}
