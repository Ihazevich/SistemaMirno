// <copyright file="WorkOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Class representing a Work Order.
    /// </summary>
    public class WorkOrder : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrder"/> class.
        /// </summary>
        public WorkOrder()
        {
            WorkOrderUnits = new Collection<WorkOrderUnit>();
        }

        /// <summary>
        /// Gets or sets the date and time the Work Order started.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time the Work Order was completed.
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Work Area where the Work Order originated from.
        /// </summary>
        [ForeignKey("OriginWorkArea")]
        public int? OriginWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area where the Work Order originated from.
        /// </summary>
        public virtual WorkArea OriginWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Work Area which the Work Order is destined to.
        /// </summary>
        [ForeignKey("DestinationWorkArea")]
        public int DestinationWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area which the Work Order is destined to.
        /// </summary>
        public virtual WorkArea DestinationWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Employee responsable for the Work Order.
        /// </summary>
        [ForeignKey("ResponsibleEmployee")]
        public int? ResponsibleEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the Employee responsable for the Work Order.
        /// </summary>
        public virtual Employee ResponsibleEmployee { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Employee in charge of supervising the Work Order.
        /// </summary>
        [ForeignKey("SupervisorEmployee")]
        public int? SupervisorEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the Employee in charge of supervising the Work Order.
        /// </summary>
        public virtual Employee SupervisorEmployee { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Order Units assigned to this Work Order.
        /// </summary>
        public virtual Collection<WorkOrderUnit> WorkOrderUnits { get; set; }
    }
}