using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class DeliveryUnit : ModelBase
    {
        [Required]
        public int DeliveryId { get; set; }

        [ForeignKey(nameof(DeliveryId))]
        public virtual Delivery Delivery { get; set; }

        [Required]
        public int WorkUnitId { get; set; }

        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }
    }
}
