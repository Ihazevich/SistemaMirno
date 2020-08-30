// <copyright file="DeliveryUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class DeliveryUnit : ModelBase
    {
        [Required]
        public int DeliveryId { get; set; }

        [ForeignKey(nameof(DeliveryId))]
        public virtual Delivery Delivery { get; set; }

        [Required]
        public int WorkUnitId { get; set; }

        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }
    }
}
