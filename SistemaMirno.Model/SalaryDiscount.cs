// <copyright file="SalaryDiscount.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a discount to an employee's salary.
    /// </summary>
    public partial class SalaryDiscount : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the discount.
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
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the description of the discount.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ammount of the discount.
        /// </summary>
        [Required]
        public long Ammount { get; set; }

        /// <summary>
        /// Gets or sets the month the discount is applied to.
        /// </summary>
        [Required]
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the year the discount is applied to.
        /// </summary>
        [Required]
        public int Year { get; set; }
    }
}
