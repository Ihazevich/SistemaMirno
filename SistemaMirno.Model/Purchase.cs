using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Purchase : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        public int? InvoiceId { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        public int? BuyOrderId { get; set; }

        [ForeignKey(nameof(BuyOrderId))]
        public virtual BuyOrder BuyOrder { get; set; }

        [Required]
        public long Ammount { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [ForeignKey(nameof(ProviderId))]
        public virtual Provider Provider { get; set; }

        [Required]
        public bool IsCredit { get; set; }

        [Required]
        public bool IsCash { get; set; }

        [Required]
        public bool IsCard { get; set; }

        public int? CreditCardId { get; set; }

        [ForeignKey(nameof(CreditCardId))]
        public virtual CreditCard CreditCard { get; set; }
    }
}
