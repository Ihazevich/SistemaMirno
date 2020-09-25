// <copyright file="DeliveryOrderReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// Represents the JSON data of a <see cref="Model.DeliveryOrder"/>.
    /// </summary>
    [Serializable]
    public class DeliveryOrderReport
    {
        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the order's responsible employee.
        /// </summary>
        public string Responsible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order was recently created.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order is a transfer order.
        /// </summary>
        public bool IsTransfer { get; set; }

        /// <summary>
        /// Gets or sets the order's origin branch.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the list of clients related to this order.
        /// </summary>
        public List<ClientReport> Clients { get; set; } = new List<ClientReport>();
    }
}
