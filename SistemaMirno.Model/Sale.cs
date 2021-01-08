// <copyright file="Sale.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a sale.
    /// </summary>
    public partial class Sale : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the sale.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [Required]
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Client"/> entity.
        /// </summary>
        [Required]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Client"/> entity.
        /// </summary>
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        /// <summary>
        /// Gets or sets the date the sale was delivered.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the expected delivery date.
        /// </summary>
        public DateTime? EstimatedDeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the total ammount of the sale before discounts.
        /// </summary>
        [Required]
        public long Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the total ammount of the sale.
        /// </summary>
        [Required]
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the discount ammmount.
        /// </summary>
        [Required]
        public long Discount { get; set; }

        /// <summary>
        /// Gets or sets the tax ammount.
        /// </summary>
        [Required]
        public long Tax { get; set; }

        /// <summary>
        /// Gets or sets the delivery fee.
        /// </summary>
        [Required]
        public long DeliveryFee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sale has an invoice or not.
        /// </summary>
        public bool HasInvoice { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Invoice"/> entity.
        /// </summary>
        public int? InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Invoice"/> entity.
        /// </summary>
        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.SaleCollection"/> entities.
        /// </summary>
        [ForeignKey(nameof(SaleCollection.SaleId))]
        public virtual ICollection<SaleCollection> SaleCollections { get; set; } = new HashSet<SaleCollection>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Requisition"/> entities.
        /// </summary>
        [ForeignKey(nameof(Requisition.SaleId))]
        public virtual ICollection<Requisition> Requisitions { get; set; } = new HashSet<Requisition>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkUnit"/> entities.
        /// </summary>
        [ForeignKey(nameof(WorkUnit.SaleId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();
    }
}
