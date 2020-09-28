// <copyright file="BankAccountMovement.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a movement from a bank account.
    /// </summary>
    public partial class BankAccountMovement : ModelBase
    {
        /// <summary>
        /// Gets or sets the bank account id.
        /// </summary>
        [Required]
        public int BankAccountId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related bank account.
        /// </summary>
        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        /// <summary>
        /// Gets or sets the date of the movement.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the description of the movement.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ammount that was deposited into the account.
        /// </summary>
        [Required]
        public long AmmountIn { get; set; }

        /// <summary>
        /// Gets or sets the ammount that was withdraw from the account.
        /// </summary>
        [Required]
        public long AmmountOut { get; set; }
    }
}
