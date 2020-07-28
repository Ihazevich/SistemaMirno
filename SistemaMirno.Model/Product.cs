﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Product : BaseModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int ProductCategoryId { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int WholesalePrice { get; set; }

        [Required]
        public int ProductionPrice { get; set; }
    }
}
