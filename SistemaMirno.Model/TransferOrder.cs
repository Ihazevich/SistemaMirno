// <copyright file="TransferOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a transfer order for moving work units between branches.
    /// </summary>
    public class TransferOrder : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Branch"/> entity
        /// that acts as the origin branch.
        /// </summary>
        [Required]
        public int FromBranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Branch"/> entity
        /// that acts as the origin branch.
        /// </summary>
        [ForeignKey(nameof(FromBranchId))]
        public virtual Branch FromBranch { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Branch"/> entity
        /// that acts as the destination branch.
        /// </summary>
        [Required]
        public int ToBranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Branch"/> entity
        /// that acts as the destination branch.
        /// </summary>
        [ForeignKey(nameof(ToBranchId))]
        public virtual Branch ToBranch { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Vehicle"/> entity.
        /// </summary>
        [Required]
        public int VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Vehicle"/> entity.
        /// </summary>
        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the work units have arrived to their destination or not.
        /// </summary>
        public bool Arrived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the work units got lost in transfer or not.
        /// </summary>
        public bool Lost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order got cancelled or not.
        /// </summary>
        public bool Cancelled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order got confirmed or not.
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.TransferUnit"/> entities.
        /// </summary>
        [ForeignKey(nameof(TransferUnit.TransferOrderId))]
        public virtual HashSet<TransferUnit> TransferUnits { get; set; } = new HashSet<TransferUnit>();
    }
}