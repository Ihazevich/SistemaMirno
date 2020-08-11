// <copyright file="ProductCategory.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing the category of a product.
    /// </summary>
    public class ProductCategory : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategory"/> class.
        /// </summary>
        public ProductCategory()
        {
            Products = new Collection<Product>();
        }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of products that are of this category.
        /// </summary>
        public virtual Collection<Product> Products { get; set; }
    }
}