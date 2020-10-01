// <copyright file="ProviderPayment.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a payment to a <see cref="Model.Provider"/>.
    /// </summary>
    public partial class ProviderPayment : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the payment.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Branch"/> entity.
        /// </summary>
        public int? BranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.DatedCheck"/> entity.
        /// </summary>
        public int? DatedCheckId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [ForeignKey(nameof(DatedCheckId))]
        public virtual DatedCheck DatedCheck { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.CreditCard"/> entity.
        /// </summary>
        public int? CreditCardId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.CreditCard"/> entity.
        /// </summary>
        [ForeignKey(nameof(CreditCardId))]
        public virtual CreditCard CreditCard { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        public int? BankAccountId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        /// <summary>
        /// Gets or sets the ammount of the payment.
        /// </summary>
        [Required]
        public long Ammount { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        [Required]
        public long ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Purchase"/> entities.
        /// </summary>
        [ForeignKey(nameof(Purchase.ProviderPaymentId))]
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
