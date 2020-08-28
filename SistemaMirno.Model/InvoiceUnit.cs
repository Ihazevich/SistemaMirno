using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class InvoiceUnit : ModelBase
    {
        [Required]
        public int InvoiceId { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public long Total { get; set; }

        [Required]
        public bool Tax10 { get; set; }
        
        [Required]
        public bool Tax5 { get; set; }
        
        [Required]
        public bool NoTax { get; set; }

        [Required]
        public bool Discount { get; set; }
    }
}
