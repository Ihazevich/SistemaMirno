// <copyright file="WorkOrderReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// A class representing the data for a work order report.
    /// </summary>
    [Serializable]
    public class WorkOrderReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderReport"/> class.
        /// </summary>
        public WorkOrderReport()
        {
            // TODO: initialize this directly with a work order and a list of work units.
            WorkUnits = new List<WorkUnitReport>();
        }

        /// <summary>
        /// Gets or sets the id of the work order.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the work order was created.
        /// </summary>
        public string CreationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the work area that created the work order.
        /// </summary>
        public string OriginWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the work area that the order is destined to.
        /// </summary>
        public string DestinationWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the full name of the employee that's responsible for the work order.
        /// </summary>
        public string Responsible { get; set; }

        /// <summary>
        /// Gets or sets the full name of the employee that's supervising the work order.
        /// </summary>
        public string Supervisor { get; set; }

        /// <summary>
        /// Gets or sets the collection of work units that the work order is moving/creating.
        /// </summary>
        public List<WorkUnitReport> WorkUnits { get; set; }
    }
}
