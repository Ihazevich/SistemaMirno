using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Hardware : ModelBase
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public int? AssignedEmployeeId { get; set; }

        [ForeignKey(nameof(AssignedEmployeeId))]
        public virtual Employee AssignedEmployee { get; set; }

        [Required]
        public bool IsWorking { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int HardwareCategoryId { get; set; }

        [ForeignKey(nameof(HardwareCategoryId))]
        public virtual HardwareCategory HardwareCategory { get; set; }

        [Required]
        public int BuyOrderId { get; set; }

        [ForeignKey(nameof(BuyOrderId))]
        public virtual BuyOrder BuyOrder { get; set; }

        [Required]
        public long Price { get; set; }
    }
}
