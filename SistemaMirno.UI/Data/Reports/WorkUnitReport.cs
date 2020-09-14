// <copyright file="WorkUnitReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// A class representing the data of a work unit.
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

        /// <summary>
        /// Gets or sets the responsible's name of the work unit.
        /// </summary>
        public string Responsible { get; set; }

        /// <summary>
        /// Gets or sets the supervisor's name of the work unit.
        /// </summary>
        public string Supervisor { get; set; }

        /// <summary>
        /// Gets or sets the client's name the work unit is assigned to.
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Gets or sets the price of the work unit.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the report includes the work unit production price.
        /// </summary>
        public bool IncludePrice { get; set; }

        /// <summary>
        /// Gets or sets the name of the current work area the unit is in.
        /// </summary>
        public string CurrentWorkArea { get; set; }
    }
}
