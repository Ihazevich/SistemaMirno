﻿// <copyright file="Color.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a product's color.
    /// </summary>
    public partial class Color : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the color.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkUnit"/> entities.
        /// </summary>
        [ForeignKey(nameof(WorkUnit.ColorId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();
    }
}