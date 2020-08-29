// <copyright file="CreditCard.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class CreditCard : ModelBase
    {
        [Required]
        public int BankAccountId { get; set; }

        [ForeignKey(nameof(BankAccountId))]
        public virtual BankAccount BankAccount { get; set; }

        [Required]
        [StringLength(4)]
        public string LastDigits { get; set; }

        [Required]
        public long Debt { get; set; }

        [ForeignKey(nameof(CreditCardPayment.CreditCardId))]
        public virtual ICollection<CreditCardPayment> CreditCardPayments { get; set; } = new HashSet<CreditCardPayment>();

        [ForeignKey(nameof(Purchase.CreditCardId))]
        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    }
}
