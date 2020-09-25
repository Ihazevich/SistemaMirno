// <copyright file="InProcessReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// Represents the JSON Data used for a In-process report.
    /// </summary>
    [Serializable]
    public class InProcessReport
    {
        /// <summary>
        /// Gets or sets the date and time the report was generated.
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// Gets or sets the work areas included in the report.
        /// </summary>
        public ICollection<WorkAreaReport> WorkAreas { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the report includes the work unit's production prices.
        /// </summary>
        public bool IncludePrice { get; set; }

        /// <summary>
        /// Gets or sets the total production value of the report.
        /// </summary>
        public long Total { get; set; }
    }
}
