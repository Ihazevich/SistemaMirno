// <copyright file="VehicleMaintenanceOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a maintenance order for a <see cref="Model.Vehicle"/> entity.
    /// </summary>
    public partial class VehicleMaintenanceOrder : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the details of the order.
        /// </summary>
        [Required]
        [StringLength(7000)]
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity
        /// that acts as the order's responsible.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property tp the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        /// <summary>
        /// Gets or sets the name of the place where the maintenance took place.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Place { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Vehicle"/> entity.
        /// </summary>
        [Required]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property tp the related <see cref="Model.Vehicle"/> entity.
        /// </summary>
        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; }
    }
}