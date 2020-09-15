using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class TransferUnit : ModelBase
    {
        [Required]
        public int TransferOrderId { get; set; }

        [ForeignKey(nameof(TransferOrderId))]
        public virtual TransferOrder TransferOrder { get; set; }

        [Required]
        public int WorkUnitId { get; set; }

        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        public bool Lost { get; set; }

        public bool Cancelled { get; set; }

        public bool Arrived { get; set; }
    }
}
