// <copyright file="BranchWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class BranchWrapper : ModelWrapper<Branch>
    {
        public BranchWrapper()
            : base(new Branch())
        {
        }

        public BranchWrapper(Branch model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        /// <summary>
        /// Gets or sets the User password.
        /// </summary>
        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string City
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Department
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public long Cash
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (Name.Length < 4)
                    {
                        yield return "El nombre es muy corto.";
                    }

                    break;

                case nameof(Address):
                    if (Address.Length < 4)
                    {
                        yield return "Dirección invalida.";
                    }

                    break;

                case nameof(City):
                    if (City.Length < 4)
                    {
                        yield return "Ciudad invalida.";
                    }

                    break;

                case nameof(Department):
                    if (Department.Length < 4)
                    {
                        yield return "Departamento invalido.";
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
