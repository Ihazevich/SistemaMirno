// <copyright file="Color.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class that represents a product's color.
    /// </summary>
    public class Color : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        public Color()
        {
            WorkUnits = new Collection<WorkUnit>();
        }

        /// <summary>
        /// Gets or sets the name of the color.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Units that have this color.
        /// </summary>
        public virtual Collection<WorkUnit> WorkUnits { get; set; }
    }
}