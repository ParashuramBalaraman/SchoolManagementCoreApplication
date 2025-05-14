using System.ComponentModel.DataAnnotations;

namespace SchoolManagementCoreApplication.ModelBOs.Ethnicities
{
    public class EthnicityBO
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
