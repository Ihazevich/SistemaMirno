using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class BuyOrderSupplyUnit : ModelBase
    {
        [Required]
        public int BuyOrderId { get; set; }

        [ForeignKey(nameof(BuyOrderId))]
        public virtual BuyOrder BuyOrder { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int SupplyId { get; set; }

        [ForeignKey(nameof(SupplyId))]
        public virtual Supply Supply { get; set; }

        [Required]
        public long IndividualPrice { get; set; }

        [Required]
        public long Total { get; set; }
    }
}
