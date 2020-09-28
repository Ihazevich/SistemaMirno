// <copyright file="BankAccount.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a single bank account.
    /// </summary>
    public partial class BankAccount : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the bank.
        /// </summary>
        [Required]
        public int BankId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the corresponding <see cref="Model.Bank"/> entity.
        /// </summary>
        [ForeignKey(nameof(BankId))]
        public virtual Bank Bank { get; set; }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        [Required]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the ammount of money currently in the bank account.
        /// </summary>
        [Required]
        public long Ammount { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="BankAccountMovement"/>
        /// entities.
        /// </summary>
        [ForeignKey(nameof(BankAccountMovement.BankAccountId))]
        public virtual ICollection<BankAccountMovement> BankAccountMovements { get; set; } = new HashSet<BankAccountMovement>();
    }
}
