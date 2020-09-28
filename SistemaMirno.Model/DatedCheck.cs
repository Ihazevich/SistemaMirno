// <copyright file="DatedCheck.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a dated check.
    /// </summary>
    public partial class DatedCheck : ModelBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the check is from the company or not.
        /// </summary>
        [Required]
        public bool IsOwnCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the check is from a client or not.
        /// </summary>
        [Required]
        public bool IsFromClient { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        [Required]
        public int? BankAccountId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        [ForeignKey(nameof(BankAccountId))]
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// Gets or sets the name of the bank that issued the check.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Bank { get; set; }

        /// <summary>
        /// Gets or sets the account number of the check.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the checkholder.
        /// </summary>
        [Required]
        public string CheckHolder { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Client"/> entity.
        /// </summary>
        [Required]
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name the check is written to.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ToName { get; set; }

        /// <summary>
        /// Gets or sets the check number.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string CheckNumber { get; set; }

        /// <summary>
        /// Gets or sets the date the check has been issued.
        /// </summary>
        [Required]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Gets or sets the check's expiration date.
        /// </summary>
        [Required]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the ammount of the check.
        /// </summary>
        [Required]
        public long Ammount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the check has been deposited.
        /// </summary>
        [Required]
        public bool Deposited { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Model.BankAccount"/> entity the check's been
        /// deposited to.
        /// </summary>
        [Required]
        public int? DepositBankAccountId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.BankAccount"/> entity.
        /// </summary>
        [ForeignKey(nameof(DepositBankAccountId))]
        public virtual BankAccount DepositBankAccount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the check has been used as a payment for
        /// a <see cref="Model.Provider"/> or not.
        /// </summary>
        [Required]
        public bool UsedAsProviderPayment { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Provider"/> entity.
        /// </summary>
        [Required]
        public int? ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Provider"/> entity.
        /// </summary>
        [ForeignKey(nameof(ProviderId))]
        public virtual Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the date the check was used.
        /// </summary>
        [Required]
        public DateTime? DateUsed { get; set; }
    }
}
