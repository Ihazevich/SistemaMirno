using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class TransferOrder : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int FromBranchId { get; set; }

        [ForeignKey(nameof(FromBranchId))]
        public virtual Branch FromBranch { get; set; }

        [Required]
        public int ToBranchId { get; set; }

        [ForeignKey(nameof(ToBranchId))]
        public virtual Branch ToBranch { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        public bool Arrived { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        [ForeignKey(nameof(TransferUnit.TransferOrderId))]
        public virtual HashSet<TransferUnit> TransferUnits { get; set; } = new HashSet<TransferUnit>();
    }
}