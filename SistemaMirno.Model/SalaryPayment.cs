﻿// <copyright file="SalaryPayment.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a salary payment.
    /// </summary>
    public partial class SalaryPayment : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the payment.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the ammount of the payment.
        /// </summary>
        [Required]
        public long Ammount { get; set; }

        /// <summary>
        /// Gets or sets the month the payment is applied to.
        /// </summary>
        [Required]
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the year the payment is applied to.
        /// </summary>
        [Required]
        public int Year { get; set; }
    }
}
