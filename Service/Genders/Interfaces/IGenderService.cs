using SchoolManagementCoreApplication.ModelBOs.Genders;

namespace SchoolManagementCoreApplication.Service.Genders.Interfaces
{
    public interface IGenderService
    {
        public GenderBO GetGender(int id);

        public List<GenderBO> GetGenders(bool active);

        public int UpsertGender(GenderBO genderBO);

        public bool DeleteGender(int id);

        public bool ToggleActive(int id);

        public IEnumerable<Tuple<string, int>> GetStudentCounts();

        public IEnumerable<Tuple<string, int>> GetTeacherCounts();

        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count);

        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count);

        public bool CheckInUse(int id);
    }
}
