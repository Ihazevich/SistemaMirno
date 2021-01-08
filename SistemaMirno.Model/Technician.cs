// <copyright file="Technician.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a technician not working for the company.
    /// </summary>
    public partial class Technician : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the technician.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the technician's phone number.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
    }
}
