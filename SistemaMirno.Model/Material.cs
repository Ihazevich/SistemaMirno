using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class Material : BaseModel
    {
        public Material()
        {
            WorkUnits = new Collection<WorkUnit>();
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public Collection<WorkUnit> WorkUnits { get; set; }
    }
}