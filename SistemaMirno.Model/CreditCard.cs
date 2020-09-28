// <copyright file="CreditCard.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a credit card.
    /// </summary>
    public class CreditCard : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        [Required]
        public int BankAccountId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        [ForeignKey(nameof(BankAccountId))]
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// Gets or sets the last 4 digits of the credit card.
        /// </summary>
        [Required]
        [StringLength(4)]
        public string LastDigits { get; set; }

        /// <summary>
        /// Gets or sets the ammount of debt in the credit card.
        /// </summary>
        [Required]
        public long Debt { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.CreditCardPayment"/> entities.
        /// </summary>
        [ForeignKey(nameof(CreditCardPayment.CreditCardId))]
        public virtual ICollection<CreditCardPayment> CreditCardPayments { get; set; } = new HashSet<CreditCardPayment>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Purchase"/> entities.
        /// </summary>
        [ForeignKey(nameof(Purchase.CreditCardId))]
        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    }
}
