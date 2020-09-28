// <copyright file="DeliveryUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a work unit in a delivery.
    /// </summary>
    public partial class DeliveryUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Delivery"/> entity.
        /// </summary>
        [Required]
        public int DeliveryId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Delivery"/> entity.
        /// </summary>
        [ForeignKey(nameof(DeliveryId))]
        public virtual Delivery Delivery { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.WorkUnit"/> entity.
        /// </summary>
        [Required]
        public int WorkUnitId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkUnit"/> entity.
        /// </summary>
        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the unit has been delivered or not.
        /// </summary>
        public bool Delivered { get; set; }

        public DateTime? DeliveredOn { get; set; }

        public bool Cancelled { get; set; }

        public string ReasonNotDelivered { get; set; }
    }
}
