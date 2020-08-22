// <copyright file="WorkOrderUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single unit in a work order.
    /// </summary>
    public class WorkOrderUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the Id of the Work Order the unit belongs to.
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Gets or sets the Work Order the unit belongs to.
        /// </summary>
        public virtual WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Work Unit the unit belongs to.
        /// </summary>
        public int WorkUnitId { get; set; }

        /// <summary>
        /// Gets or sets the Work Unit the unit belongs to.
        /// </summary>
        public virtual WorkUnit WorkUnit { get; set; }
    }
}
