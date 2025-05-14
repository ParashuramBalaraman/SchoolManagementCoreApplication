using System.ComponentModel.DataAnnotations;

namespace SchoolManagementCoreApplication.ModelBOs.Student
{
    public class StudentBO
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

        public string Address { get; set; }

        public int GenderId { get; set; }

        public int EthnicityId { get; set; }

        public bool Active { get; set; }
    }
}
