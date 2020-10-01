// <copyright file="HardwareMaintenanceOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a maintenance order for a hardware unit.
    /// </summary>
    public partial class HardwareMaintenanceOrder : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Hardware"/> entity.
        /// </summary>
        [Required]
        public int HardwareId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Hardware"/> entity.
        /// </summary>
        [ForeignKey(nameof(HardwareId))]
        public virtual Hardware Hardware { get; set; }

        /// <summary>
        /// Gets or sets the description of the order.
        /// </summary>
        [Required]
        [StringLength(7000)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Technician"/> entity.
        /// </summary>
        [Required]
        public int TechnicianId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Technician"/> entity.
        /// </summary>
        [ForeignKey(nameof(TechnicianId))]
        public virtual Technician Technician { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int SupervisorId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee Supervisor { get; set; }
    }
}
