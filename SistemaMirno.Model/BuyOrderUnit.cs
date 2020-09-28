// <copyright file="BuyOrderUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents an item in a <see cref="Model.BuyOrder"/>.
    /// </summary>
    public partial class BuyOrderUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the <see cref="Model.BuyOrder"/> this item belongs to.
        /// </summary>
        [Required]
        public int BuyOrderId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Model.BuyOrder"/> related to
        /// the buy order unit.
        /// </summary>
        [ForeignKey(nameof(BuyOrderId))]
        public virtual BuyOrder BuyOrder { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the item.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        [Required]
        public int Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        [Required]
        public long Price { get; set; }

        /// <summary>
        /// Gets or sets the total ammount of money of the item.
        /// </summary>
        [Required]
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Model.Supply"/> the item is related to.
        /// </summary>
        public int? SupplyId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Model.Supply"/> entity
        /// related to the item.
        /// </summary>
        [ForeignKey(nameof(SupplyId))]
        public virtual Supply Supply { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Model.Hardware"/> the item is related to.
        /// </summary>
        public int? HardwareId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Model.Hardware"/> entity
        /// related to the item.
        /// </summary>
        [ForeignKey(nameof(HardwareId))]
        public virtual Hardware Hardware { get; set; }
    }
}
