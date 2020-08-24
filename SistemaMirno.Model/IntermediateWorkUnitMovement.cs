using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class IntermediateWorkUnitMovement : ModelBase
    {
        [Required]
        public int IntermediateProductId { get; set; }

        [ForeignKey(nameof(IntermediateProductId))]
        public virtual IntermediateProduct IntermediateProduct { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int QuantityIn { get; set; }

        [Required]
        public int QuantityOut { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }
    }
}
