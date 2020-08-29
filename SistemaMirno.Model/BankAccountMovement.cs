// <copyright file="BankAccountMovement.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class BankAccountMovement : ModelBase
    {
        [Required]
        public int BankAccountId { get; set; }

        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public long AmmountIn { get; set; }

        [Required]
        public long AmmountOut { get; set; }
    }
}
