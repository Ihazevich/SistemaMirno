// <copyright file="ProductPart.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a part of a product.
    /// </summary>
    public partial class ProductPart : ModelBase
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
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the ammount of this part that go into the product.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the description of the part.
        /// </summary>
        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the part material.
        /// </summary>
        [Required]
        public string Material { get; set; }

        /// <summary>
        /// Gets or sets the length of the part before processing.
        /// </summary>
        [Required]
        public double RawLength { get; set; }

        /// <summary>
        /// Gets or sets the width of the part before processing.
        /// </summary>
        [Required]
        public double RawWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the part before processing.
        /// </summary>
        [Required]
        public double RawHeight { get; set; }

        /// <summary>
        /// Gets or sets the length of the part after processing.
        /// </summary>
        [Required]
        public double FinishedLength { get; set; }

        /// <summary>
        /// Gets or sets the width of the part after processing.
        /// </summary>
        [Required]
        public double FinishedWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the part after processing.
        /// </summary>
        [Required]
        public double FinishedHeight { get; set; }

        /// <summary>
        /// Gets or sets the location of the image file for the part.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ImageFile { get; set; }
    }
}
