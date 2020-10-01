// <copyright file="IntermediateWorkUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a unit of work in an <see cref="Model.IntermediateWorkOrder"/>.
    /// </summary>
    public partial class IntermediateWorkUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the ammount of intermediate products in the work unit.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.IntermediateProduct"/> entity.
        /// </summary>
        [Required]
        public int IntermediateProductId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.IntermediateProduct"/> entity.
        /// </summary>
        [ForeignKey(nameof(IntermediateProductId))]
        public virtual IntermediateProduct IntermediateProduct { get; set; }
    }
}
