using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class IntermediateProduct : ModelBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int ManufacturingWorkAreaId { get; set; }

        [ForeignKey(nameof(ManufacturingWorkAreaId))]
        public WorkArea ManufacturingWorkArea { get; set; }
    }
}
