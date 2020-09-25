// <copyright file="ClientReport.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// Represents the JSON data of a <see cref="Model.Client"/>.
    /// </summary>
    [Serializable]
    public class ClientReport
    {
        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the city where the cient is located.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the address of the client.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the client.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the list of WorkUnits related to the client.
        /// </summary>
        public List<WorkUnitReport> WorkUnits { get; set; } = new List<WorkUnitReport>();
    }
}
