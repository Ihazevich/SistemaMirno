// <copyright file="Employee.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a single employee in the company.
    /// </summary>
    public partial class Employee : ModelBase
    {
        /// <summary>
        /// Gets or sets the first name of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets the full name of the employee.
        /// </summary>
        [NotMapped]
        public string FullName => FirstName + " " + LastName;

        /// <summary>
        /// Gets or sets the document number of the employee.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the birth date of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the age of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the address of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets navigation property to the related <see cref="Model.Role"/> entities.
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();

        /// <summary>
        /// Gets or sets the base salary of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public long BaseSalary { get; set; }

        /// <summary>
        /// Gets or sets the salary extra bonus of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public long SalaryOtherBonus { get; set; }

        /// <summary>
        /// Gets or sets the salary production bonus of the employee.
        /// </summary>
        [Required]
        public long SalaryProductionBonus { get; set; }

        /// <summary>
        /// Gets or sets the salary sales bonus of the employee.
        /// </summary>
        [Required]
        public long SalarySalesBonus { get; set; }

        /// <summary>
        /// Gets or sets the salary work order bonus of the employee.
        /// </summary>
        [Required]
        public long SalaryWorkOrderBonus { get; set; }

        /// <summary>
        /// Gets or sets the salary normal hours bonus of the employee.
        /// </summary>
        [Required]
        public long SalaryNormalHoursBonus { get; set; }

        /// <summary>
        /// Gets or sets the salary extra hours bonus of the employee.
        /// </summary>
        [Required]
        public long SalaryExtraHoursBonus { get; set; }

        /// <summary>
        /// Gets or sets the total salary of the employee.
        /// </summary>
        [Required]
        public long TotalSalary { get; set; }

        /// <summary>
        /// Gets or sets the reported salary of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public long ReportedIpsSalary { get; set; }

        /// <summary>
        /// Gets or sets the production bonus ratio of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public double ProductionBonusRatio { get; set; }

        /// <summary>
        /// Gets or sets the sales bonus ratio of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public double SalesBonusRatio { get; set; }

        /// <summary>
        /// Gets or sets the price per normal hour of work of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public long PricePerNormalHour { get; set; }

        /// <summary>
        /// Gets or sets the price per extra hour of work of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public long PricePerExtraHour { get; set; }

        /// <summary>
        /// Gets or sets the contract's start date of the employee.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public DateTime ContractStartDate { get; set; }

        /// <summary>
        /// Gets or sets the location of contract file.
        /// </summary>
        public string ContractFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the employee is registered in IPS or not.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public bool IsRegisteredInIps { get; set; }

        /// <summary>
        /// Gets or sets the date the employee was registered in ips.
        /// </summary>
        public DateTime? IpsStartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the employee is terminated.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public bool Terminated { get; set; }

        /// <summary>
        /// Gets or sets the date the employee's contract was terminated.
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.User"/> entity.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.User"/> entity.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Assistance"/> entities.
        /// </summary>
        [ForeignKey(nameof(Assistance.EmployeeId))]
        public virtual ICollection<Assistance> Assistances { get; set; } = new HashSet<Assistance>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.SalaryPayment"/> entities.
        /// </summary>
        [ForeignKey(nameof(SalaryPayment.EmployeeId))]
        public virtual ICollection<SalaryPayment> SalaryPayments { get; set; } = new HashSet<SalaryPayment>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.SalaryDiscount"/> entities.
        /// </summary>
        [ForeignKey(nameof(SalaryDiscount.EmployeeId))]
        public virtual ICollection<SalaryDiscount> SalaryDiscounts { get; set; } = new HashSet<SalaryDiscount>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.HistoricalSalary"/> entities.
        /// </summary>
        [ForeignKey(nameof(HistoricalSalary.EmployeeId))]
        public virtual ICollection<HistoricalSalary> HistoricalSalaries { get; set; } = new HashSet<HistoricalSalary>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkOrder"/> entities.
        /// </summary>
        [ForeignKey(nameof(WorkOrder.ResponsibleEmployeeId))]
        public virtual ICollection<WorkOrder> WorkOrders { get; set; } = new HashSet<WorkOrder>();
   }
}