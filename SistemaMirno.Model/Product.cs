// <copyright file="Product.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a single product.
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
        /// Gets or sets the id of the related <see cref="Model.ProductCategory"/> entity.
        /// </summary>
        [Required]
        public int ProductCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.ProductCategory"/> entity.
        /// </summary>
        [ForeignKey(nameof(ProductCategoryId))]
        public virtual ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// Gets or sets the production value, used for production reports.
        /// </summary>
        [Required]
        public long ProductionValue { get; set; }

        /// <summary>
        /// Gets or sets the product' price for retail clients.
        /// </summary>
        [Required]
        public long RetailPrice { get; set; }

        /// <summary>
        /// Gets or sets the product's price for wholesaler clients.
        /// </summary>
        [Required]
        public long WholesalerPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is custom or not.
        /// </summary>
        [Required]
        public bool IsCustom { get; set; }

        /// <summary>
        /// Gets or sets the location of the sketchup file.
        /// </summary>
        [StringLength(100)]
        public string SketchupFile { get; set; }

        /// <summary>
        /// Gets or sets the location of the template file.
        /// </summary>
        [StringLength(100)]
        public string TemplateFile { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkUnit"/> entities.
        /// </summary>
        [ForeignKey(nameof(WorkUnit.ProductId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.ProductPicture"/> entities.
        /// </summary>
        [ForeignKey(nameof(ProductPicture.ProductId))]
        public virtual ICollection<ProductPicture> ProductPictures { get; set; } = new HashSet<ProductPicture>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.ProductPart"/> entities.
        /// </summary>
        [ForeignKey(nameof(ProductPart.ProductId))]
        public virtual ICollection<ProductPart> ProductParts { get; set; } = new HashSet<ProductPart>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.ProductSupply"/> entities.
        /// </summary>
        [ForeignKey(nameof(ProductSupply.ProductId))]
        public virtual ICollection<ProductSupply> ProductSupplies { get; set; } = new HashSet<ProductSupply>();
    }
}