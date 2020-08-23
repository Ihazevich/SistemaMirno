using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class HardwareMaintenanceOrder : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int HardwareId { get; set; }

        [ForeignKey(nameof(HardwareId))]
        public virtual Hardware Hardware { get; set; }

        [Required]
        [StringLength(7000)]
        public string Description { get; set; }

        [Required]
        public int TechnicianId { get; set; }

        [ForeignKey(nameof(TechnicianId))]
        public virtual Technician Technician { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee Supervisor { get; set; }
    }
}
