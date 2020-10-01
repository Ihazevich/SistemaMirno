// <copyright file="Purchase.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a purchase.
    /// </summary>
    public partial class Purchase : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the purchase.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Invoice"/> entity.
        /// </summary>
        public int? InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Invoice"/> entity.
        /// </summary>
        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.BuyOrder"/> entity.
        /// </summary>
        public int? BuyOrderId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.BuyOrder"/> entity.
        /// </summary>
        [ForeignKey(nameof(BuyOrderId))]
        public virtual BuyOrder BuyOrder { get; set; }

        /// <summary>
        /// Gets or sets the ammount of the purchase.
        /// </summary>
        [Required]
        public long Ammount { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Provider"/> entity.
        /// </summary>
        [Required]
        public int ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Provider"/> entity.
        /// </summary>
        [ForeignKey(nameof(ProviderId))]
        public virtual Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.ProviderPayment"/> entity.
        /// </summary>
        public int? ProviderPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.ProviderPayment"/> entity.
        /// </summary>
        [ForeignKey(nameof(ProviderPaymentId))]
        public ProviderPayment ProviderPayment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the purchase is a credit purchase or not.
        /// </summary>
        [Required]
        public bool IsCredit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the purchase is a cash purchase or not.
        /// </summary>
        [Required]
        public bool IsCash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the purchase is a card purchase or not.
        /// </summary>
        [Required]
        public bool IsCard { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.CreditCard"/> entity.
        /// </summary>
        public int? CreditCardId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.CreditCard"/> entity.
        /// </summary>
        [ForeignKey(nameof(CreditCardId))]
        public virtual CreditCard CreditCard { get; set; }
    }
}
