// <copyright file="Supply.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a supply.
    /// </summary>
    public partial class Supply : ModelBase
    {
        /// <summary>
        /// Gets or sets the quantity of the supply currently in inventory.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the name of the supply.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unit used for measuring the supply quantities.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the id of the realted <see cref="Model.SupplyCategory"/> entity.
        /// </summary>
        [Required]
        public int SupplyCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Model.SupplyCategory"/> entity.
        /// </summary>
        [ForeignKey(nameof(SupplyCategoryId))]
        public virtual SupplyCategory SupplyCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an order should be printed when withdrawing
        /// the supply from the inventory.
        /// </summary>
        public bool RequiresOrderToWithdraw { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.SupplyMovement"/> entities.
        /// </summary>
        [ForeignKey(nameof(SupplyMovement.SupplyId))]
        public virtual ICollection<SupplyMovement> SupplyMovements { get; set; }
    }
}
