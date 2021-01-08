// <copyright file="Vehicle.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a company vehicle used for logistics.
    /// </summary>
    public partial class Vehicle : ModelBase
    {
        /// <summary>
        /// Gets or sets the vehicle's model.
        /// </summary>
        [Required]
        public string VehicleModel { get; set; }

        /// <summary>
        /// Gets or sets the vehicle's year.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the vehicle's patent number.
        /// </summary>
        public string Patent { get; set; }

        /// <summary>
        /// Gets the name of the vehicle using the model and the patent number.
        /// </summary>
        [NotMapped]
        public string FullName => string.Concat(VehicleModel, " - ", Patent);

        /// <summary>
        /// Gets or sets a value indicating whether the vehicle is available for deliveries or not.
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the renovation date for the patent.
        /// </summary>
        public DateTime PatentExpiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the patent has been paid this year.
        /// </summary>
        public bool PatentPaid { get; set; }

        /// <summary>
        /// Gets or sets the renovation date for the dinatran documents.
        /// </summary>
        public DateTime DinatranExpiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dinatran documents have been paid this year.
        /// </summary>
        public bool DinatranPaid { get; set; }

        /// <summary>
        /// Gets or sets the renovation date for the fire extinguishers.
        /// </summary>
        public DateTime FireExtinguisherExpiration { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.VehicleMaintenanceOrder"/> entities.
        /// </summary>
        [ForeignKey(nameof(VehicleMaintenanceOrder.VehicleId))]
        public ICollection<VehicleMaintenanceOrder> VehicleMaintenanceOrders { get; set; } = new HashSet<VehicleMaintenanceOrder>();
    }
}
