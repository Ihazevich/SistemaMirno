// <copyright file="Employee.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a single employee in the company.
    /// </summary>
    public partial class Employee : ModelBase
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        [MaxLength(30)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => FirstName + " " + LastName;

        /// <summary>
        /// Gets or sets the document number(CI) of the employee.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido")]
        public string DocumentNumber { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [StringLength(30)]
        public string Profession { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();

        [Required(ErrorMessage = "Requerido")]
        public long BaseSalary { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public long SalaryOtherBonus { get; set; }
        
        [Required]
        public long SalaryProductionBonus { get; set; }
        
        [Required]
        public long SalarySalesBonus { get; set; }
        
        [Required]
        public long SalaryWorkOrderBonus { get; set; }

        [Required]
        public long SalaryNormalHoursBonus { get; set; }

        [Required]
        public long SalaryExtraHoursBonus { get; set; }

        [Required]
        public long TotalSalary { get; set; }
        
        [Required(ErrorMessage = "Requerido")]
        public long ReportedIpsSalary { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public double ProductionBonusRatio { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public double SalesBonusRatio { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public long PricePerNormalHour { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public long PricePerExtraHour { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public DateTime ContractStartDate { get; set; }

        public string ContractFile { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public bool IsRegisteredInIps { get; set; }

        public DateTime? IpsStartDate { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public bool Terminated { get; set; }

        public DateTime? TerminationDate { get; set; }

        public int? UserId { get; set; }
        
        public virtual User User { get; set; }

        [ForeignKey(nameof(Assistance.EmployeeId))]
        public virtual ICollection<Assistance> Assistances { get; set; } = new HashSet<Assistance>();

        [ForeignKey(nameof(SalaryPayment.EmployeeId))]
        public virtual ICollection<SalaryPayment> SalaryPayments { get; set; } = new HashSet<SalaryPayment>();

        [ForeignKey(nameof(SalaryDiscount.EmployeeId))]
        public virtual ICollection<SalaryDiscount> SalaryDiscounts { get; set; } = new HashSet<SalaryDiscount>();

        [ForeignKey(nameof(HistoricalSalary.EmployeeId))]
        public virtual ICollection<HistoricalSalary> HistoricalSalaries { get; set; } = new HashSet<HistoricalSalary>();
   }
}