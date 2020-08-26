// <copyright file="WorkUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a product with a certain material and color.
    /// </summary>
    public partial class WorkUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the Id of the product of the work unit.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// gets or sets the product of the work unit.
        /// </summary>
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the Id of the material of the work unit.
        /// </summary>
        [Required]
        public int MaterialId { get; set; }

        /// <summary>
        /// Gets or sets the material of the work unit.
        /// </summary>
        [ForeignKey(nameof(MaterialId))]
        public virtual Material Material { get; set; }

        /// <summary>
        /// Gets or sets the Id of the color of the work unit.
        /// </summary>
        [Required]
        public int ColorId { get; set; }

        /// <summary>
        /// Gets or sets the color of the work unit.
        /// </summary>
        [ForeignKey(nameof(ColorId))]
        public virtual Color Color { get; set; }

        /// <summary>
        /// Gets or sets the id of the work area the work unit is presently in.
        /// </summary>
        [Required]
        public int CurrentWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the work area the work unit is presently in.
        /// </summary>
        [ForeignKey(nameof(CurrentWorkAreaId))]
        public virtual WorkArea CurrentWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the id of the requisition the work unit has been assigned to.
        /// </summary>
        public int? RequisitionId { get; set; }

        /// <summary>
        /// Gets or sets the work unit's requisition.
        /// </summary>
        [ForeignKey(nameof(RequisitionId))]
        public virtual Requisition Requisition { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public double TotalWorkTime { get; set; }

        [Required]
        public bool Delivered { get; set; }

        [Required]
        public int LatestResponsibleId { get; set; }

        [ForeignKey(nameof(LatestResponsibleId))]
        public virtual Employee LatestResponsible { get; set; }

        [Required]
        public int LatestSupervisorId { get; set; }

        [ForeignKey(nameof(LatestSupervisorId))]
        public virtual Employee LatestSupervisor { get; set; }

        public string Details { get; set; }
    }
}