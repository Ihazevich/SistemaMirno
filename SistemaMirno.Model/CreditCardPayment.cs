// <copyright file="CreditCardPayment.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class CreditCardPayment : ModelBase
    {
        [Required]
        public int CreditCardId { get; set; }

        [ForeignKey(nameof(CreditCardId))]
        public virtual CreditCard CreditCard { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long Ammount { get; set; }
    }
}
