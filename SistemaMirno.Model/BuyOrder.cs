using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class BuyOrder : ModelBase
    {
        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        [Required]
        public long Total { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User User { get; set; }

        [Required]
        public string OrderNumber { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [ForeignKey(nameof(ProviderId))]
        public virtual Provider Provider { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        [ForeignKey(nameof(Hardware.BuyOrderId))]
        public virtual ICollection<Hardware> HardwareUnits { get; set; } = new HashSet<Hardware>();

        [ForeignKey(nameof(BuyOrderSupplyUnit.BuyOrderId))]
        public virtual ICollection<BuyOrderSupplyUnit> SupplyUnits { get; set; } = new HashSet<BuyOrderSupplyUnit>();
    }
}
