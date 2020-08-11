// <copyright file="Material.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a product's material.
    /// </summary>
    public class Material : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        public Material()
        {
            WorkUnits = new Collection<WorkUnit>();
        }

        /// <summary>
        /// Gets or sets the name of the material.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Units that have this material.
        /// </summary>
        public virtual Collection<WorkUnit> WorkUnits { get; set; }
    }
}