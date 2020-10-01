// <copyright file="IntermediateProduct.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents an intermediate product.
    /// </summary>
    public partial class IntermediateProduct : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.WorkArea"/> entity.
        /// </summary>
        [Required]
        public int ManufacturingWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkArea"/> entity.
        /// </summary>
        [ForeignKey(nameof(ManufacturingWorkAreaId))]
        public WorkArea ManufacturingWorkArea { get; set; }
    }
}
