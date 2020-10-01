// <copyright file="WorkOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Class representing a Work Order.
    /// </summary>
    public partial class WorkOrder : ModelBase
    {
        /// <summary>
        /// Gets or sets the date and time the Work Order started.
        /// </summary>
        [Required]
        public DateTime CreationDateTime { get; set; }

        public DateTime? FinishedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Work Area where the Work Order originated from.
        /// </summary>
        [Required]
        public int OriginWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area where the Work Order originated from.
        /// </summary>
        [ForeignKey(nameof(OriginWorkAreaId))]
        public virtual WorkArea OriginWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Work Area which the Work Order is destined to.
        /// </summary>
        [Required]
        public int DestinationWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area which the Work Order is destined to.
        /// </summary>
        [ForeignKey(nameof(DestinationWorkAreaId))]
        public virtual WorkArea DestinationWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Employee responsable for the Work Order.
        /// </summary>
        [Required]
        public int ResponsibleEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the Employee responsable for the Work Order.
        /// </summary>
        [ForeignKey(nameof(ResponsibleEmployeeId))]
        public virtual Employee ResponsibleEmployee { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Employee in charge of supervising the Work Order.
        /// </summary>
        [Required]
        public int SupervisorEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the Employee in charge of supervising the Work Order.
        /// </summary>
        [ForeignKey(nameof(SupervisorEmployeeId))]
        public virtual Employee SupervisorEmployee { get; set; }

        [StringLength(200)]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Order Units assigned to this Work Order.
        /// </summary>
        [ForeignKey(nameof(WorkOrderUnit.WorkOrderId))]
        public virtual ICollection<WorkOrderUnit> WorkOrderUnits { get; set; } = new HashSet<WorkOrderUnit>();
    }
}