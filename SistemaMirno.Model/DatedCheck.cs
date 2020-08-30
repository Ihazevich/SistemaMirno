// <copyright file="DatedCheck.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class DatedCheck : ModelBase
    {
        [Required]
        public bool IsOwnCheck { get; set; }

        [Required]
        public bool IsFromClient { get; set; }

        [Required]
        public int? BankAccountId { get; set; }

        [ForeignKey(nameof(BankAccountId))]
        public virtual BankAccount BankAccount { get; set; }

        [Required]
        [StringLength(100)]
        public string Bank { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountNumber { get; set; }

        [Required]
        public string CheckHolder { get; set; }

        [Required]
        public int? ClientId { get; set; }

        [Required]
        [StringLength(200)]
        public string ToName { get; set; }

        [Required]
        [StringLength(100)]
        public string CheckNumber { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public long Ammount { get; set; }

        [Required]
        public bool Deposited { get; set; }

        [Required]
        public int? DepositBankAccountId { get; set; }

        [ForeignKey(nameof(DepositBankAccountId))]
        public virtual BankAccount DepositBankAccount { get; set; }

        [Required]
        public bool UsedAsProviderPayment { get; set; }

        [Required]
        public int? ProviderId { get; set; }

        [ForeignKey(nameof(ProviderId))]
        public virtual Provider Provider { get; set; }

        [Required]
        public DateTime? DateUsed { get; set; }
    }
}
