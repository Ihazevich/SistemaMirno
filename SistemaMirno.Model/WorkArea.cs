// <copyright file="WorkArea.cs" company="HazeLabs">
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
    /// A class representing a work area.
    /// </summary>
    public class WorkArea : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int Position { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [Required]
        public int ResponsibleRoleId { get; set; }

        [ForeignKey(nameof(ResponsibleRoleId))]
        public Role ResponsibleRole { get; set; }

        [Required]
        public int SupervisorRoleId { get; set; }

        [ForeignKey(nameof(SupervisorRoleId))]
        public Role SupervisorRole { get; set; }

        [Required]
        public bool ReportsInProgress { get; set; }

        [ForeignKey(nameof(WorkAreaMovement.WorkAreaId))]
        public virtual ICollection<WorkAreaMovement> WorkAreaMovements { get; set; } = new HashSet<WorkAreaMovement>();
    }
}