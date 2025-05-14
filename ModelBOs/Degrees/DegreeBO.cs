using System.ComponentModel.DataAnnotations;

namespace SchoolManagementCoreApplication.ModelBOs.Degrees
{
    public class DegreeBO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int DepartmentId{ get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
