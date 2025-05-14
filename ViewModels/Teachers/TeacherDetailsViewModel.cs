using SchoolManagementCoreApplication.ModelBOs.Teacher;

namespace SchoolManagementCoreApplication.ViewModels.Teacher
{
    public class TeacherDetailsViewModel
    {
        public TeacherBO Teacher { get; set; }
        public IEnumerable<string> StudentNames { get; set; }
    }
}
