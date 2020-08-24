using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Provider : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }


        [ForeignKey(nameof(BuyOrder.ProviderId))]
        public virtual ICollection<BuyOrder> BuyOrders { get; set; } = new HashSet<BuyOrder>();


        [ForeignKey(nameof(DatedCheck.ProviderId))]
        public virtual ICollection<DatedCheck> DatedChecks { get; set; } = new HashSet<DatedCheck>();
    }
}
