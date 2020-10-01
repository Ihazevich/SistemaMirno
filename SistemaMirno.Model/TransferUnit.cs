// <copyright file="TransferUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class TransferUnit : ModelBase
    {
        [Required]
        public int TransferOrderId { get; set; }

        [ForeignKey(nameof(TransferOrderId))]
        public virtual TransferOrder TransferOrder { get; set; }

        [Required]
        public int WorkUnitId { get; set; }

        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        public int FromWorkAreaId { get; set; }

        [ForeignKey(nameof(FromWorkAreaId))]
        public virtual WorkArea FromWorkArea { get; set; }

        public int ToWorkAreaId { get; set; }

        [ForeignKey(nameof(ToWorkAreaId))]
        public virtual WorkArea ToWorkArea { get; set; }

        public bool Lost { get; set; }

        public bool Cancelled { get; set; }

        public bool Arrived { get; set; }
    }
}
