// <copyright file="CreditCardPayment.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a single credit card payment.
    /// </summary>
    public class CreditCardPayment : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.CreditCard"/> entity.
        /// </summary>
        [Required]
        public int CreditCardId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.CreditCard"/> entity.
        /// </summary>
        [ForeignKey(nameof(CreditCardId))]
        public virtual CreditCard CreditCard { get; set; }

        /// <summary>
        /// Gets or sets the date of the payment.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the ammount of the payment.
        /// </summary>
        [Required]
        public long Ammount { get; set; }
    }
}
