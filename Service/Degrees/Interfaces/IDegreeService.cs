using SchoolManagementCoreApplication.ModelBOs.Degrees;
using SchoolManagementCoreApplication.ModelBOs.Teacher;

namespace SchoolManagementCoreApplication.Service.Degrees.Interfaces
{
    public interface IDegreeService
    {
        public DegreeBO GetDegree(int id);

        public List<DegreeBO> GetDegrees(bool active);

        public int UpsertDegree(DegreeBO degreeBO);

        public bool DeleteDegree(int id);

        public bool ToggleActive(int id);

        public IEnumerable<TeacherBO> GetTeachersByDegree(int degreeId);

        public IEnumerable<Tuple<string, int>> GetStudentCounts(int count);

        public IEnumerable<Tuple<string, float>> GetTeacherCounts();

        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count);

        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count);

        public bool CheckInUse(int id);
    }
}
