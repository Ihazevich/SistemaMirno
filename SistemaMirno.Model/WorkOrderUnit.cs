// <copyright file="WorkOrderUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single unit in a work order.
    /// </summary>
    public partial class WorkOrderUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the Id of the Work Order the unit belongs to.
        /// </summary>
        [Required]
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Gets or sets the Work Order the unit belongs to.
        /// </summary>
        [ForeignKey(nameof(WorkOrderId))]
        public virtual WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Work Unit the unit belongs to.
        /// </summary>
        [Required]
        public int WorkUnitId { get; set; }

        /// <summary>
        /// Gets or sets the Work Unit the unit belongs to.
        /// </summary>
        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        public DateTime? FinishedDateTime { get; set; }
    }
}
