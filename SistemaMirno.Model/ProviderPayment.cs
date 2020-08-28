using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class ProviderPayment : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        public int? DatedCheckId { get; set; }

        [ForeignKey(nameof(DatedCheckId))]
        public virtual DatedCheck DatedCheck { get; set; }

        public int? CreditCardId { get; set; }

        [ForeignKey(nameof(CreditCardId))]
        public virtual CreditCard CreditCard { get; set; }

        public int? BankAccountId { get; set; }

        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        [Required]
        public long Ammount { get; set; }

        [Required]
        public long ReceiptNumber { get; set; }

        [ForeignKey(nameof(Purchase.ProviderPaymentId))]
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
