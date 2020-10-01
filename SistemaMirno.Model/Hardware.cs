// <copyright file="Hardware.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a hardware piece.
    /// </summary>
    public partial class Hardware : ModelBase
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        [Required]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        [Required]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        public int? AssignedEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(AssignedEmployeeId))]
        public virtual Employee AssignedEmployee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the hardware is working or not.
        /// </summary>
        [Required]
        public bool IsWorking { get; set; }

        /// <summary>
        /// Gets or sets the date the hardware stopped working.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.HardwareCategory"/> entity.
        /// </summary>
        public int HardwareCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.HardwareCategory"/> entity.
        /// </summary>
        [ForeignKey(nameof(HardwareCategoryId))]
        public virtual HardwareCategory HardwareCategory { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        [Required]
        public long Price { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.HardwareMaintenanceOrder"/> entities.
        /// </summary>
        [ForeignKey(nameof(HardwareMaintenanceOrder.HardwareId))]
        public virtual ICollection<HardwareMaintenanceOrder> HardwareMaintenanceOrders { get; set; } = new HashSet<HardwareMaintenanceOrder>();
    }
}
