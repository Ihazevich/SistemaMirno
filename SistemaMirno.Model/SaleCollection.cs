// <copyright file="SaleCollection.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a sale payment.
    /// </summary>
    public partial class SaleCollection : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Sale"/> entity.
        /// </summary>
        [Required]
        public int SaleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Sale"/> entity.
        /// </summary>
        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        /// <summary>
        /// Gets or sets the date of the payment.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the paid ammount.
        /// </summary>
        [Required]
        public int Ammount { get; set; }

        /// <summary>
        /// Gets or sets the description of the payment.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this payment is in fact a discount or not,
        /// in which case its ammount should be discounted from the sale but not added to
        /// any cash flow or account.
        /// </summary>
        [Required]
        public bool IsDiscount { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.DatedCheck"/> entity.
        /// </summary>
        [Required]
        public int? DatedCheckId { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        [Required]
        public int? BankAccountId { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [Required]
        public int? BranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Sale"/> entity.
        /// </summary>
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }
    }
}
