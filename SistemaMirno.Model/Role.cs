// <copyright file="EmployeeRole.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single employee role.
    /// </summary>
    public class Role : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }

        [Required]
        public bool HasAccessToSales { get; set; }

        [Required]
        public bool HasAccessToProduction { get; set; }

        [Required]
        public bool HasAccessToHumanResources { get; set; }

        [Required]
        public bool HasAccessToAccounting { get; set; }

        [Required]
        public bool HasAccessToLogistics { get; set; }

        [Required]
        public bool IsSystemAdmin { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
