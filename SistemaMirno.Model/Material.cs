using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class Material : BaseModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<WorkUnit> WorkUnits { get; set; }
    }
}