// <copyright file="SupplyMovement.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents an supply inventory movement.
    /// </summary>
    public partial class SupplyMovement : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the movement.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Supply"/> entity.
        /// </summary>
        [Required]
        public int SupplyId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Supply"/> entity.
        /// </summary>
        [ForeignKey(nameof(SupplyId))]
        public virtual Supply Supply { get; set; }

        /// <summary>
        /// Gets or sets the description of the movement.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity deposited into the inventory.
        /// </summary>
        [Required]
        public int InQuantity { get; set; }

        /// <summary>
        /// Gets or sets the quantity withdrawed from the inventory.
        /// </summary>
        [Required]
        public int OutQuantity { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }
    }
}
