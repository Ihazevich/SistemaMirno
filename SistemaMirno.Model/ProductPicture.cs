// <copyright file="ProductPicture.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents the picture of a product.
    /// </summary>
    public partial class ProductPicture : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Product"/> entity.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Product"/> entity.
        /// </summary>
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets the location of the image file.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ImageFile { get; set; }
    }
}
