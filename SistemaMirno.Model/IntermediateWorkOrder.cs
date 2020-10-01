// <copyright file="IntermediateWorkOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a work order for an intermediate product.
    /// </summary>
    public partial class IntermediateWorkOrder : ModelBase
    {
        /// <summary>
        /// Gets or sets the date the order was created.
        /// </summary>
        [Required]
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date the order was finished.
        /// </summary>
        public DateTime? FinisheDateTime { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity
        /// that acts as responsible for the order.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity
        /// that acts as responsible for the order.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity
        /// that acts as supervisor for the order.
        /// </summary>
        [Required]
        public int SupervisorId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity
        /// that acts as supervisor for the order.
        /// </summary>
        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee Supervisor { get; set; }

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

        /// <summary>
        /// Gets or sets the quantity of the order.
        /// </summary>
        [Required]
        public int Quantity { get; set; }
    }
}
