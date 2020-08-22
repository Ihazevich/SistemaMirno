// <copyright file="Product.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single product.
    /// </summary>
    public class Product : ModelBase
    {
        // TODO: Change prices names. Retail price, production value, wholesaler price.

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
            WorkUnits = new Collection<WorkUnit>();
        }

        /// <summary>
        /// Gets or sets the code of the product.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Id of the product's category.
        /// </summary>
        [Required]
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the product's category.
        /// </summary>
        public virtual ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// Gets or sets the product's price for the general public.
        /// </summary>
        [Required]
        public int Price { get; set; }

        /// <summary>
        /// Gets or sets the product's price for wholesalers.
        /// </summary>
        [Required]
        public int WholesalePrice { get; set; }

        /// <summary>
        /// Gets or sets the product's price for production reports.
        /// </summary>
        [Required]
        public int ProductionPrice { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Units that have this product as base.
        /// </summary>
        public virtual Collection<WorkUnit> WorkUnits { get; set; }
    }
}