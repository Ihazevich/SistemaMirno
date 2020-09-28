// <copyright file="DeliveryOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a single delivery order.
    /// </summary>
    public partial class DeliveryOrder : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Vehicle"/> entity.
        /// </summary>
        [Required]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Vehicle"/> entity.
        /// </summary>
        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order has been confirmed or not.
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// Gets or sets the km in the delivery vehicle before the delivery.
        /// </summary>
        [Required]
        public int KmBefore { get; set; }

        /// <summary>
        /// Gets or sets the km in the delivery vehicle after the delivery.
        /// </summary>
        [Required]
        public int KmAfter { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Delivery"/> entity.
        /// </summary>
        [ForeignKey(nameof(Delivery.DeliveryOrderId))]
        public virtual ICollection<Delivery> Deliveries { get; set; } = new HashSet<Delivery>();
    }
}
