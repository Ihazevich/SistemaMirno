﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class SaleCollection : ModelBase
    {
        [Required]
        public int SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Ammount { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string ReceiptNumber { get; set; }

        [Required]
        public bool IsDiscount { get; set; }

        [Required]
        public int? DatedCheckId { get; set; }
        
        [Required]
        public int? BankAccountId { get; set; }

        [Required]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }
    }
}
