// <copyright file="HardwareCategory.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a hardware category.
    /// </summary>
    public partial class HardwareCategory : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Hardware"/> entities.
        /// </summary>
        [ForeignKey(nameof(Hardware.HardwareCategoryId))]
        public virtual ICollection<Hardware> Hardwares { get; set; } = new HashSet<Hardware>();
    }
}
