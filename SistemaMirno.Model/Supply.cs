// <copyright file="Supply.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class Supply : ModelBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        public string MeasureUnit { get; set; }

        [Required]
        public int SupplyCategoryId { get; set; }

        [ForeignKey(nameof(SupplyCategoryId))]
        public virtual SupplyCategory SupplyCategory { get; set; }
    }
}
