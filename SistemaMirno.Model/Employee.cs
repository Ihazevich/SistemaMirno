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
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }


        [NotMapped]
        public string FullName => FirstName + " " + LastName;

        /// <summary>
        /// Gets or sets the document number(CI) of the employee.
        /// </summary>
        [Required]
        public int DocumentNumber { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int Age { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string Profession { get; set; }

        [Required]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }

        [Required]
        public long BaseSalary { get; set; }
        
        [Required]
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
        
        [Required]
        public long ReportedIpsSalary { get; set; }
        
        [Required]
        public double ProductionBonusRatio { get; set; }
        
        [Required]
        public double SalesBonusRatio { get; set; }
        
        [Required]
        public long PricePerNormalHour { get; set; }

        [Required]
        public long PricePerExtraHour { get; set; }

        [Required]
        public DateTime ContractStartDate { get; set; }

        public string ContractFile { get; set; }

        public bool IsRegisteredInIps { get; set; }

        public DateTime? IpsStartDate { get; set; }
        
        public bool Terminated { get; set; }

        public DateTime? TerminationDate { get; set; }

        public int UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Assistance> Assistances { get; set; } = new HashSet<Assistance>();

        public virtual ICollection<SalaryPayment> SalaryPayments { get; set; } = new HashSet<SalaryPayment>();

        public virtual ICollection<SalaryDiscount> SalaryDiscounts { get; set; } = new HashSet<SalaryDiscount>();

        public virtual ICollection<HistoricalSalary> HistoricalSalaries { get; set; } = new HashSet<HistoricalSalary>();
   }
}