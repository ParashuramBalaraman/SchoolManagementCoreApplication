using System.ComponentModel.DataAnnotations;

namespace SchoolManagementCoreApplication.ModelBOs.StudentAllocation
{
    public class StudentAllocationBO
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int DegreeId { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }
}
