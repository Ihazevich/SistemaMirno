// <copyright file="EmployeeRole.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single employee role.
    /// </summary>
    public class EmployeeRole : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of employees that have been assigned to this role.
        /// </summary>
        public virtual Collection<Employee> Employees { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Areas that have this role assigned as the responsible role of that Work Area.
        /// </summary>
        [InverseProperty("WorkAreaResponsibleRole")]
        public virtual Collection<WorkArea> WorkAreasResponsibles { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Areas that have this role assigned as the supervisor role of that Work Area.
        /// </summary>
        [InverseProperty("WorkAreaSupervisorRole")]
        public virtual Collection<WorkArea> WorkAreasSupervisors { get; set; }
    }
}
