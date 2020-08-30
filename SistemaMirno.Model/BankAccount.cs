// <copyright file="BankAccount.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class BankAccount : ModelBase
    {
        [Required]
        public int BankId { get; set; }

        [ForeignKey(nameof(BankId))]
        public virtual Bank Bank { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public long Ammount { get; set; }

        [ForeignKey(nameof(BankAccountMovement.BankAccountId))]
        public virtual ICollection<BankAccountMovement> BankAccountMovements { get; set; } = new HashSet<BankAccountMovement>();
    }
}
