// <copyright file="Sale.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class Sale : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        [Required]
        public long Subtotal { get; set; }

        [Required]
        public long Total { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long Tax { get; set; }

        [Required]
        public long DeliveryFee { get; set; }

        public bool HasInvoice { get; set; }

        public int? InvoiceId { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public virtual Invoice Invoice { get; set; }

        [ForeignKey(nameof(SaleCollection.SaleId))]
        public virtual ICollection<SaleCollection> SaleCollections { get; set; } = new HashSet<SaleCollection>();

        [ForeignKey(nameof(Requisition.SaleId))]
        public virtual ICollection<Requisition> Requisitions { get; set; } = new HashSet<Requisition>();

        [ForeignKey(nameof(WorkUnit.SaleId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();
    }
}
