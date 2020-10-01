// <copyright file="HistoricalSalary.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a historical salary registry of an employee.
    /// </summary>
    public partial class HistoricalSalary : ModelBase
    {
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
        /// Gets or sets the month of the registry.
        /// </summary>
        [Required]
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the year of the registry.
        /// </summary>
        [Required]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the base salary.
        /// </summary>
        [Required]
        public long Base { get; set; }

        /// <summary>
        /// Gets or sets the reported salary to ips.
        /// </summary>
        [Required]
        public long ReportedIpsSalary { get; set; }

        /// <summary>
        /// Gets or sets the ammount paid to ips by the company.
        /// </summary>
        [Required]
        public long CompanyIpsAmmount { get; set; }

        /// <summary>
        /// Gets or sets the ammount paid to ips by the employee.
        /// </summary>
        [Required]
        public long EmployeeIpsAmmount { get; set; }

        /// <summary>
        /// Gets or sets the production bonus ratio.
        /// </summary>
        [Required]
        public double ProductionBonusRatio { get; set; }

        /// <summary>
        /// Gets or sets the sales bonus ratio.
        /// </summary>
        [Required]
        public double SalesBonusRatio { get; set; }

        /// <summary>
        /// Gets or sets the price per normal hour of work.
        /// </summary>
        [Required]
        public long PricePerNormalHour { get; set; }

        /// <summary>
        /// Gets or sets the price per extra hour of work.
        /// </summary>
        [Required]
        public long PricePerExtraHour { get; set; }

        /// <summary>
        /// Gets or sets the sales bonus ammount.
        /// </summary>
        [Required]
        public long SalesBonus { get; set; }

        /// <summary>
        /// Gets or sets the production bonus ammount.
        /// </summary>
        [Required]
        public long ProductionBonus { get; set; }

        /// <summary>
        /// Gets or sets the work order bonus ammount.
        /// </summary>
        [Required]
        public long WorkOrdersBonus { get; set; }

        /// <summary>
        /// Gets or sets the normal hours bonus ammount.
        /// </summary>
        [Required]
        public long NormalHoursBonus { get; set; }

        /// <summary>
        /// Gets or sets the extra hours bonus ammount.
        /// </summary>
        [Required]
        public long ExtraHoursBonus { get; set; }

        /// <summary>
        /// Gets or sets the other bonus ammount.
        /// </summary>
        [Required]
        public long OtherBonus { get; set; }

        /// <summary>
        /// Gets or sets the total discounted ammount.
        /// </summary>
        [Required]
        public long TotalDiscounts { get; set; }

        /// <summary>
        /// Gets the total salary.
        /// </summary>
        public long Total => Base + SalesBonus + ProductionBonus + WorkOrdersBonus + NormalHoursBonus + ExtraHoursBonus + OtherBonus - TotalDiscounts;
    }
}
