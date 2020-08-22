// <copyright file="Product.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single product.
    /// </summary>
    public partial class Product : ModelBase
    {
        /// <summary>
        /// Gets or sets the code of the product.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Id of the product's category.
        /// </summary>
        [Required]
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the product's category.
        /// </summary>
        [ForeignKey(nameof(ProductCategoryId))]
        public virtual ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// Gets or sets the product's price for the general public.
        /// </summary>
        [Required]
        public long ProductionValue { get; set; }

        /// <summary>
        /// Gets or sets the product's price for wholesalers.
        /// </summary>
        [Required]
        public long RetailPrice { get; set; }

        /// <summary>
        /// Gets or sets the product's price for production reports.
        /// </summary>
        [Required]
        public long WholesalerPrice { get; set; }

        [Required]
        public bool IsCustom { get; set; }

        [Required]
        [StringLength(100)]
        public string SketchupFile { get; set; }

        [Required]
        [StringLength(7000)]
        public string TemplateDetails { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Units that have this product as base.
        /// </summary>
        [ForeignKey(nameof(WorkUnit.ProductId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();

        [ForeignKey(nameof(ProductPicture.ProductId))]
        public virtual ICollection<ProductPicture> ProductPictures { get; set; } = new HashSet<ProductPicture>();

        [ForeignKey(nameof(ProductPart.ProductId))]
        public virtual ICollection<ProductPart> ProductParts { get; set; } = new HashSet<ProductPart>();
    }
}