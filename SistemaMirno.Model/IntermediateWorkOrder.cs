using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class IntermediateWorkOrder : ModelBase
    {
        [Required]
        public DateTime CreationDateTime { get; set; }

        public DateTime? FinisheDateTime { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee Supervisor { get; set; }

        [Required]
        public int IntermediateProductId { get; set; }

        [ForeignKey(nameof(IntermediateProductId))]
        public virtual IntermediateProduct IntermediateProduct { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
