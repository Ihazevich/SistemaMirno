// <copyright file="Assistance.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class that represents the assistance data gotten from an assistance clock.
    /// </summary>
    public partial class Assistance : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the employee that clocked this assistance.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Model.Employee"/> entity related to this assistance.
        /// </summary>
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the date this assistance belongs to.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets time at which the employee checked in.
        /// </summary>
        public string CheckIn { get; set; }

        /// <summary>
        /// Gets or sets time at which the employee checked out.
        /// </summary>
        public string CheckOut { get; set; }
    }
}
