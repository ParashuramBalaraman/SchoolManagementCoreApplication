using System.ComponentModel.DataAnnotations;

namespace SchoolManagementCoreApplication.ModelBOs.Teacher
{
    public class TeacherBO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int GenderId { get; set; }

        [Required]
        public int EthnicityId { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int DegreeId { get; set; }
    }
}
