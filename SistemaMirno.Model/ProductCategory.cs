// <copyright file="ProductCategory.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents the category of a <see cref="Model.Product"/>.
    /// </summary>
    public partial class ProductCategory : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.ProductCategory"/> entities.
        /// </summary>
        [ForeignKey(nameof(Product.ProductCategoryId))]
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}