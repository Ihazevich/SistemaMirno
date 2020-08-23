using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class SupplyMovement : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int SupplyId { get; set; }

        [ForeignKey(nameof(SupplyId))]
        public virtual Supply Supply { get; set; }

        [Required]
        public int InQuantity { get; set; }

        [Required]
        public int OutQuantity { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }
    }
}
