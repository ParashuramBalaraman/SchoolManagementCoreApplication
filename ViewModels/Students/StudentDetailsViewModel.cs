using SchoolManagementCoreApplication.ModelBOs.Student;

namespace SchoolManagementCoreApplication.ViewModels.Students
{
    public class StudentDetailsViewModel
    {
        public StudentBO Student { get; set; }
        public IEnumerable<string> DegreeNames { get; set; }
        public IEnumerable<string> TeacherNames { get; set; }
    }
}
