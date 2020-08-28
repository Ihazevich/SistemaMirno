using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class CreditCardPayment : ModelBase
    {
        [Required]
        public int CreditCardId { get; set; }

        [ForeignKey(nameof(CreditCardId))]
        public virtual CreditCard CreditCard { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long Ammount { get; set; }
    }
}
