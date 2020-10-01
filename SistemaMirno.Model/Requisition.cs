// <copyright file="Requisition.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a work unit requisition.
    /// </summary>
    public class Requisition : ModelBase
    {
        /// <summary>
        /// Gets or sets the date the requisition was created.
        /// </summary>
        [Required]
        public DateTime RequestedDate { get; set; }

        /// <summary>
        /// Gets or sets the priority of the requisition.
        /// </summary>
        [Required]
        public string Priority { get; set; }

        /// <summary>
        /// Gets or sets the target date to fulfill the requisition.
        /// </summary>
        public DateTime? TargetDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the requisition is already fulfilled or not.
        /// </summary>
        [Required]
        public bool Fulfilled { get; set; }

        /// <summary>
        /// Gets or sets the date the requisition was fulfilled.
        /// </summary>
        public DateTime? FulfilledDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the requisition is for stock or not.
        /// </summary>
        [Required]
        public bool IsForStock { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Client"/> entity.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Client"/> entity.
        /// </summary>
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Sale"/> entity.
        /// </summary>
        public int? SaleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Sale"/> entity.
        /// </summary>
        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkUnit"/> entities.
        /// </summary>
        [ForeignKey(nameof(WorkUnit.RequisitionId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();
    }
}
