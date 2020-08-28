using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Invoice : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Ruc { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public long Tax10 { get; set; }

        [Required]
        public long Tax5 { get; set; }

        [Required]
        public long TotalTax { get; set; }

        [Required]
        public long Total { get; set; }

        [ForeignKey(nameof(InvoiceUnit.InvoiceId))]
        public virtual ICollection<InvoiceUnit> InvoiceUnits { get; set; } = new HashSet<InvoiceUnit>();

        [Required]
        public bool IsPaid { get; set; }
    }
}
