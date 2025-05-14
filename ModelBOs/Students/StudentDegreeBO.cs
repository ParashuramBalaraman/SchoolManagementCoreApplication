using System.ComponentModel.DataAnnotations;

namespace SchoolManagementCoreApplication.ModelBOs.StudentDegree
{
    public class StudentDegreeBO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int DegreeId { get; set; }
    }
}
