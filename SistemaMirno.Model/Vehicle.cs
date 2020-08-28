using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Vehicle : ModelBase
    {
        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Odometer { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public DateTime PatentExpiration { get; set; }

        [Required]
        public bool PatentPaid { get; set; }

        [Required]
        public DateTime DinatranExpiration { get; set; }

        [Required]
        public bool DinatranPaid { get; set; }

        [Required]
        public DateTime FireExtinguisherExpiration { get; set; }

        [ForeignKey(nameof(VehicleMaintenanceOrder.VehicleId))]
        public ICollection<VehicleMaintenanceOrder> VehicleMaintenanceOrders { get; set; } = new HashSet<VehicleMaintenanceOrder>();
    }
}
