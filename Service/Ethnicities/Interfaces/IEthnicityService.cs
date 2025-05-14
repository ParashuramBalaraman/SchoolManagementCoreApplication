using SchoolManagementCoreApplication.ModelBOs.Ethnicities;

namespace SchoolManagementCoreApplication.Service.Ethnicities.Interfaces
{
    public interface IEthnicityService
    {
        public EthnicityBO GetEthnicity(int id);

        public List<EthnicityBO> GetEthnicities(bool active);

        public int UpsertEthnicity(EthnicityBO ethnicityBO);

        public bool DeleteEthnicity(int id);

        public bool ToggleActive(int id);

        public IEnumerable<Tuple<string, int>> GetStudentCounts();

        public IEnumerable<Tuple<string, int>> GetTeacherCounts();

        public IEnumerable<Tuple<string, float>> GetStudentPercentages(float count);

        public IEnumerable<Tuple<string, float>> GetTeacherPercentages(float count);

        public bool CheckInUse(int id);
    }
}
