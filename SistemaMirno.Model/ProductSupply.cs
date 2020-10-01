// <copyright file="ProductSupply.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a supply especification for a product.
    /// </summary>
    public partial class ProductSupply : ModelBase
    {
        /// <summary>
        /// Gets or sets the ammount of the supply that goes into the product.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

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
        /// Gets or sets the id of the related <see cref="Model.Product"/> entity.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Product"/> entity.
        /// </summary>
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
