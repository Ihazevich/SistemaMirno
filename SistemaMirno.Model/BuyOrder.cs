// <copyright file="BuyOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class BuyOrder : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(100)]
        public string OrderNumber { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [ForeignKey(nameof(ProviderId))]
        public virtual Provider Provider { get; set; }

        [Required]
        public long Total { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        [ForeignKey(nameof(BuyOrderUnit.BuyOrderId))]
        public virtual ICollection<BuyOrderUnit> BuyOrderUnits { get; set; } = new HashSet<BuyOrderUnit>();

        [Required]
        public bool IsPaid { get; set; }
    }
}
