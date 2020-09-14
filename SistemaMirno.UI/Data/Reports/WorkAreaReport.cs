// <copyright file="WorkAreaReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// A class representing the data of a work area used for reports.
    /// </summary>
    [Serializable]
    public class WorkAreaReport
    {
        /// <summary>
        /// Gets or sets the name of the work area.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the report includes the production price of work units.
        /// </summary>
        public bool IncludePrice { get; set; }

        /// <summary>
        /// Gets or sets the total production value of the work area.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the work units in the work area.
        /// </summary>
        public List<WorkUnitReport> WorkUnits { get; set; } = new List<WorkUnitReport>();
    }
}
