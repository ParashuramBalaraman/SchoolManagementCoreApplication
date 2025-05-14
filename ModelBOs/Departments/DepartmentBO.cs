using System.ComponentModel.DataAnnotations;

namespace SchoolManagementCoreApplication.ModelBOs.Departments
{
    public class DepartmentBO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
