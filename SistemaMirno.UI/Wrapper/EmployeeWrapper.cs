// <copyright file="EmployeeWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
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

        public int Id => GetValue<int>();

        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string FullName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DocumentNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime BirthDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int Age
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Address
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string PhoneNumber
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public long BaseSalary
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long SalaryOtherBonus
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long SalaryProductionBonus
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long SalarySalesBonus
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long SalaryWorkOrderBonus
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long SalaryNormalHoursBonus
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long SalaryExtraHoursBonus
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long TotalSalary
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long ReportedIpsSalary
        {
            get { return GetValue<long>(); }
            set { SetValue(value); }
        }

        public double ProductionBonusRatio
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public double SalesBonusRatio
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public long PricePerNormalHour
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long PricePerExtraHour
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public DateTime ContractStartDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string ContractFile
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsRegisteredInIps
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public DateTime? IpsStartDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public bool Terminated
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public DateTime? TerminationDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public int? UserId
        {
            get => GetValue<int?>();
            set => SetValue(value);
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
                    if (!int.TryParse(DocumentNumber, out int _))
                    {
                        yield return "Numero de documento invalido.";
                    }

                    break;
            }

            foreach (var error in base.ValidateProperty(propertyName))
            {
                if (error != null)
                {
                    yield return error;
                }
            }
        }
    }
}
