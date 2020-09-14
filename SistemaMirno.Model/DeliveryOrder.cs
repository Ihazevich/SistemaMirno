// <copyright file="DeliveryOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class DeliveryOrder : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        public bool Confirmed { get; set; }

        [Required]
        public int KmBefore { get; set; }

        [Required]
        public int KmAfter { get; set; }

        [ForeignKey(nameof(Delivery.DeliveryOrderId))]
        public virtual ICollection<Delivery> Deliveries { get; set; } = new HashSet<Delivery>();
    }
}
