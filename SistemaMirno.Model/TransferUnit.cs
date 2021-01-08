// <copyright file="TransferUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a work unit in a <see cref="Model.TransferOrder"/>.
    /// </summary>
    public class TransferUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.TransferOrder"/> entity.
        /// </summary>
        [Required]
        public int TransferOrderId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.TransferOrder"/> entity.
        /// </summary>
        [ForeignKey(nameof(TransferOrderId))]
        public virtual TransferOrder TransferOrder { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.WorkUnit"/> entity.
        /// </summary>
        [Required]
        public int WorkUnitId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkUnit"/> entity.
        /// </summary>
        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.WorkArea"/> entity
        /// that acts as the origin area.
        /// </summary>
        public int FromWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkArea"/> entity
        /// that acts as the origin area.
        /// </summary>
        [ForeignKey(nameof(FromWorkAreaId))]
        public virtual WorkArea FromWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.WorkArea"/> entity
        /// that acts as the destination area.
        /// </summary>
        public int ToWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.WorkArea"/> entity
        /// that acts as the destination area.
        /// </summary>
        [ForeignKey(nameof(ToWorkAreaId))]
        public virtual WorkArea ToWorkArea { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the work unit got lost in transit or not.
        /// </summary>
        public bool Lost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the work unit got cancelled from the order or not.
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the work unit arrived at the destination or not.
        /// </summary>
        public bool Arrived { get; set; }
    }
}
