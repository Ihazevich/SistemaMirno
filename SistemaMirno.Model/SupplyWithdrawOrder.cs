// <copyright file="SupplyWithdrawOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class SupplyWithdrawOrder : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee Supervisor { get; set; }

        [ForeignKey(nameof(SupplyWithdrawOrderUnit.SupplyWithdrawOrderId))]
        public ICollection<SupplyWithdrawOrderUnit> SupplyWithdrawOrderUnits { get; set; }
    }
}
