using SchoolManagementCoreApplication.ModelBOs.Departments;

namespace SchoolManagementCoreApplication.Service.Departments.Interfaces
{
    public interface IDepartmentService
    {
        public DepartmentBO GetDepartment(int id);

        public List<DepartmentBO> GetDepartments(bool active);

        public int UpsertDepartment(DepartmentBO departmentBO);

        public bool DeleteDepartment(int id);

        public bool ToggleActive(int id);

        public IEnumerable<Tuple<string, int>> GetStudentCounts();

        public IEnumerable<Tuple<string, int>> GetTeacherCounts();

        public IEnumerable<Tuple<string, int>> GetDegreeCounts();

        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count);

        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count);

        public IEnumerable<Tuple<string, float>> GetDegreePercentages(float count);

        public bool CheckInUse(int id);
    }
}
