// <copyright file="SupplyWithdrawOrderUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class SupplyWithdrawOrderUnit : ModelBase
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public int SupplyId { get; set; }

        [ForeignKey(nameof(SupplyId))]
        public virtual Supply Supply { get; set; }

        [Required]
        public int SupplyWithdrawOrderId { get; set; }

        [ForeignKey(nameof(SupplyWithdrawOrderId))]
        public virtual SupplyWithdrawOrder SupplyWithdrawOrder { get; set; }
    }
}
