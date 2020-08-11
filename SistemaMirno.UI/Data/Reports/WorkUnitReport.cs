// <copyright file="WorkUnitReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// A class representing the data for a work unit report.
    /// </summary>
    [Serializable]
    public class WorkUnitReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitReport"/> class.
        /// </summary>
        public WorkUnitReport()
        {
            // TODO: initialize with a work unit instance.
        }

        /// <summary>
        /// Gets or sets the quantity for the work unit.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the name of the product of the work unit.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the name of the material of the work unit.
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// Gets or sets the name of the color of the work unit.
        /// </summary>
        public string Color { get; set; }
    }
}
