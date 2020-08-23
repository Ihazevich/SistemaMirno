using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class SaleCollection : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Ammount { get; set; }

        [Required]
        public int SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }
    }
}
