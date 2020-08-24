using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class CashMovement : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public long AmmountIn { get; set; }

        [Required]
        public long AmmountOut { get; set; }
    }
}
