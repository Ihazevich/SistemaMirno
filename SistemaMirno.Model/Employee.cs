// <copyright file="Employee.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single employee in the company.
    /// </summary>
    public class Employee : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class.
        /// </summary>
        public Employee()
        {
            ResponsibleWorkOrders = new Collection<WorkOrder>();
            SupervisorWorkOrders = new Collection<WorkOrder>();
        }

        /// <summary>
        /// Gets or sets the first name of the employee.
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the employee.
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the document number(CI) of the employee.
        /// </summary>
        public int DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the date the employee started working at the company.
        /// </summary>
        public DateTime HiredDate { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Role the employee currently has.
        /// </summary>
        public int EmployeeRoleId { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Orders that the employee is/was responsible for.
        /// </summary>
        [InverseProperty("ResponsibleEmployee")]
        public virtual Collection<WorkOrder> ResponsibleWorkOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Orders that the employee is/was supervising.
        /// </summary>
        [InverseProperty("SupervisorEmployee")]
        public virtual Collection<WorkOrder> SupervisorWorkOrders { get; set; }

        // TODO: Add SaleOrders
    }
}