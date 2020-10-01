// <copyright file="IntermediateWorkUnitMovement.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents the movement of an intermediate work unit.
    /// </summary>
    public partial class IntermediateWorkUnitMovement : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.IntermediateProduct"/> entity.
        /// </summary>
        [Required]
        public int IntermediateProductId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the related <see cref="Model.IntermediateProduct"/> entity.
        /// </summary>
        [ForeignKey(nameof(IntermediateProductId))]
        public virtual IntermediateProduct IntermediateProduct { get; set; }

        /// <summary>
        /// Gets or sets the date of the movement.
        /// </summary>
        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the ammount that entered the inventory.
        /// </summary>
        [Required]
        public int QuantityIn { get; set; }

        /// <summary>
        /// Gets or sets the ammount that left the inventory.
        /// </summary>
        [Required]
        public int QuantityOut { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity
        /// that acts as responsible of the movement.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity
        /// that acts as responsible of the movement.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }
    }
}
