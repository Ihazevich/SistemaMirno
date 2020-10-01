// <copyright file="Invoice.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents an invoice.
    /// </summary>
    public class Invoice : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the invoice.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the ruc identifier of the invoice recipient.
        /// </summary>
        [Required]
        public string Ruc { get; set; }

        /// <summary>
        /// Gets or sets the name of the invoice recipient.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code of the invoice.
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the total 10% tax of the invoice.
        /// </summary>
        [Required]
        public long Tax10 { get; set; }

        /// <summary>
        /// Gets or sets the total 5% tax of the invoice.
        /// </summary>
        [Required]
        public long Tax5 { get; set; }

        /// <summary>
        /// Gets or sets the total tax of the invoice.
        /// </summary>
        [Required]
        public long TotalTax { get; set; }

        /// <summary>
        /// Gets or sets the total ammount of the invoice.
        /// </summary>
        [Required]
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.InvoiceUnit"/> entities.
        /// </summary>
        [ForeignKey(nameof(InvoiceUnit.InvoiceId))]
        public virtual ICollection<InvoiceUnit> InvoiceUnits { get; set; } = new HashSet<InvoiceUnit>();

        /// <summary>
        /// Gets or sets a value indicating whether the invoice has been paid or not.
        /// </summary>
        [Required]
        public bool IsPaid { get; set; }
    }
}
