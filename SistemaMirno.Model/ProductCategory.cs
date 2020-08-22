// <copyright file="ProductCategory.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing the category of a product.
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
        /// Gets or sets the collection of products that are of this category.
        /// </summary>
        [ForeignKey(nameof(Product.ProductCategoryId))]
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}