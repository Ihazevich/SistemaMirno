// <copyright file="Bank.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class that represents bank information.
    /// </summary>
    public partial class Bank : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Model.BankAccount"/> entities related to this bank.
        /// </summary>
        [ForeignKey(nameof(BankAccount.BankId))]
        public virtual ICollection<BankAccount> BankAccounts { get; set; } = new HashSet<BankAccount>();
    }
}
