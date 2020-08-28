using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public partial class Technician : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
