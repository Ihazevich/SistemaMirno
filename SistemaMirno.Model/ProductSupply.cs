﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class ProductSupply : ModelBase
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public int SupplyId { get; set; }

        [ForeignKey(nameof(SupplyId))]
        public virtual Supply Supply { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
