using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class EmployeeWrapper : ModelWrapper<Employee>
    {
        public EmployeeWrapper()
            : base(new Employee())
        {
        }

        public EmployeeWrapper(Employee model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string FullName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string DocumentNumber
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime BirthDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public int Age
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string PhoneNumber
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Profession
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public long BaseSalary
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long SalaryOtherBonus
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long SalaryProductionBonus
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long SalarySalesBonus
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long SalaryWorkOrderBonus
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long SalaryNormalHoursBonus
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long SalaryExtraHoursBonus
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long TotalSalary
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long ReportedIpsSalary
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public double ProductionBonusRatio
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public double SalesBonusRatio
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public long PricePerNormalHour
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public long PricePerExtraHour
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public DateTime ContractStartDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public string ContractFile
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool IsRegisteredInIps
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public DateTime? IpsStartDate
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public bool Terminated
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public DateTime? TerminationDate
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public int? UserId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (FirstName.Length < 4)
                    {
                        yield return "El nombre es muy corto.";
                    }

                    break;

                case nameof(LastName):
                    if (LastName.Length < 3)
                    {
                        yield return "Apellido muy corto.";
                    }

                    break;

                case nameof(DocumentNumber):
                    if (!int.TryParse(DocumentNumber,out int _))
                    {
                        yield return "Numero de documento invalido.";
                    }

                    break;
            }
        }
    }
}
