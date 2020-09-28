// <copyright file="CashMovement.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a cash movement from a <see cref="Model.Branch"/>.
    /// </summary>
    public partial class CashMovement : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the movement.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Model.Branch"/> related to the movement.
        /// </summary>
        [Required]
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property of the <see cref="Model.Branch"/> entity
        /// related to the movement.
        /// </summary>
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        /// <summary>
        /// Gets or sets the description of the movement.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ammount deposited in to the branch.
        /// </summary>
        [Required]
        public long AmmountIn { get; set; }

        /// <summary>
        /// Gets or sets the ammount withdrawed from the branch.
        /// </summary>
        [Required]
        public long AmmountOut { get; set; }
    }
}
