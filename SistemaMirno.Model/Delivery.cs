// <copyright file="Delivery.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a single delivery.
    /// </summary>
    public partial class Delivery : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Sale"/> entity.
        /// </summary>
        [Required]
        public int SaleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Sale"/> entity.
        /// </summary>
        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.DeliveryOrder"/> entity.
        /// </summary>
        [Required]
        public int DeliveryOrderId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.DeliveryOrder"/> entity.
        /// </summary>
        [ForeignKey(nameof(DeliveryOrderId))]
        public virtual DeliveryOrder DeliveryOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the delivery has been delivered or not.
        /// </summary>
        public bool Delivered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the delivery has been cancelled or not.
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// Gets or sets the date the delivery was completed.
        /// </summary>
        public DateTime? DeliveredOn { get; set; }

        /// <summary>
        /// Gets or sets the details of the delivery.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the reason the delivery wasn't completed.
        /// </summary>
        public string ReasonNotDelivered { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.DeliveryUnit"/> entities.
        /// </summary>
        public virtual ICollection<DeliveryUnit> DeliveryUnits { get; set; } = new HashSet<DeliveryUnit>();
    }
}
