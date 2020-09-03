// <copyright file="ProductionReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data.Reports
{
    [Serializable]
    public class ProductionReport
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string WorkArea { get; set; }

        public long Total { get; set; }

        public ICollection<WorkUnitReport> WorkUnits { get; set; }
    }
}
