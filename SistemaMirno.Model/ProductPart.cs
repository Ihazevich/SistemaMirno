// <copyright file="ProductPart.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class ProductPart : ModelBase
    {
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        [Required]
        public string Material { get; set; }

        [Required]
        public double RawLength { get; set; }

        [Required]
        public double RawWidth { get; set; }

        [Required]
        public double RawHeight { get; set; }

        [Required]
        public double FinishedLength { get; set; }

        [Required]
        public double FinishedWidth { get; set; }

        [Required]
        public double FinishedHeight { get; set; }

        [Required]
        [StringLength(100)]
        public string ImageFile { get; set; }
    }
}
