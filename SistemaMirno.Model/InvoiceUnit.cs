// <copyright file="InvoiceUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents an item in an <see cref="Model.Invoice"/>.
    /// </summary>
    public class InvoiceUnit : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Invoice"/> entity.
        /// </summary>
        [Required]
        public int InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Invoice"/> entity.
        /// </summary>
        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the item.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        [Required]
        public long Price { get; set; }

        /// <summary>
        /// Gets or sets the total value ammount of the item.
        /// </summary>
        [Required]
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item has 10% tax or not.
        /// </summary>
        [Required]
        public bool Tax10 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item has 5% tax or not.
        /// </summary>
        [Required]
        public bool Tax5 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is tax exempt or not.
        /// </summary>
        [Required]
        public bool NoTax { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item's value should be applied as a discount or not.
        /// </summary>
        [Required]
        public bool Discount { get; set; }
    }
}
