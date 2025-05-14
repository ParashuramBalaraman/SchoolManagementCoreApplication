using SchoolManagementCoreApplication.ModelBOs.Teacher;

namespace SchoolManagementCoreApplication.Service.Teachers.Interfaces
{
    public interface ITeacherService
    {
        public TeacherBO GetTeacher(int id);

        public List<TeacherBO> GetTeachers(bool active);

        public int UpsertTeacher(TeacherBO teacherBO);

        public bool DeleteTeacher(int id);

        public bool ToggleActive(int id);

        public bool CheckInUse(int id);
    }
}
