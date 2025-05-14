using SchoolManagementCoreApplication.ModelBOs.Degrees;

namespace SchoolManagementCoreApplication.ViewModels.Degree
{
    public class DegreeDetailsViewModel
    {
        public DegreeBO Degree { get; set; }
        public IEnumerable<string> StudentNames { get; set; }
        public IEnumerable<string> TeacherNames { get; set; }
    }
}
