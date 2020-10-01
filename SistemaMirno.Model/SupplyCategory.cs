// <copyright file="SupplyCategory.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a supply category.
    /// </summary>
    public partial class SupplyCategory : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the supply.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
