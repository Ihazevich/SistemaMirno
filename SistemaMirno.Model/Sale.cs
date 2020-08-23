﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

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
        public long Total { get; set; }

        [Required]
        public long Discount { get; set; }

        [Required]
        public long Tax { get; set; }
        
        [Required]
        public long DeliveryFee { get; set; }

        [ForeignKey(nameof(SaleCollection.SaleId))]
        public virtual ICollection<SaleCollection> SaleCollections { get; set; }

        [ForeignKey(nameof(Requisition.SaleId))]
        public virtual Requisition SaleRequisition { get; set; }
    }
}
