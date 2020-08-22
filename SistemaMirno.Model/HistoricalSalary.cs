using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class HistoricalSalary : ModelBase
    {
        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public long Base { get; set; }

        [Required]
        public long ReportedIpsSalary { get; set; }

        [Required]
        public long CompanyIpsAmmount { get; set; }

        [Required]
        public long EmployeeIpsAmmount { get; set; }

        [Required]
        public double ProductionBonusRatio { get; set; }

        [Required]
        public double SalesBonusRatio { get; set; }

        [Required]
        public long PricePerNormalHour { get; set; }

        [Required]
        public long PricePerExtraHour { get; set; }

        [Required]
        public long SalesBonus { get; set; }

        [Required]
        public long ProductionBonus { get; set; }

        [Required]
        public long WorkOrdersBonus { get; set; }

        [Required]
        public long NormalHoursBonus { get; set; }

        [Required]
        public long ExtraHoursBonus { get; set; }

        [Required]
        public long OtherBonus { get; set; }

        [Required]
        public long TotalDiscounts { get; set; }

        public long Total => Base + SalesBonus + ProductionBonus + WorkOrdersBonus + NormalHoursBonus + ExtraHoursBonus + OtherBonus - TotalDiscounts;
    }
}
