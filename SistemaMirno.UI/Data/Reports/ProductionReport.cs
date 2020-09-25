// <copyright file="ProductionReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// Represents the JSON data of a production report.
    /// </summary>
    [Serializable]
    public class ProductionReport
    {
        /// <summary>
        /// Gets or sets the start date of the report.
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// Gets or sets the final date of the report.
        /// </summary>
        public string ToDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the Work Area the report is from.
        /// </summary>
        public string WorkArea { get; set; }

        /// <summary>
        /// Gets or sets the total value of the work units in the report.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the list of work units related to the report.
        /// </summary>
        public ICollection<WorkUnitReport> WorkUnits { get; set; } = new List<WorkUnitReport>();
    }
}
