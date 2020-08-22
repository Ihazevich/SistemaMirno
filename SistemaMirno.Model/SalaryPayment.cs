// <copyright file="SalaryPayments.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class SalaryPayment : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        [Required]
        public long Ammount { get; set; }

        [Required]
        public int Month { get; set; }
        
        [Required]
        public int Year { get; set; }
    }
}
