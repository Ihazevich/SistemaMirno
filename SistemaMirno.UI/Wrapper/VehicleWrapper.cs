// <copyright file="VehicleWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class VehicleWrapper : ModelWrapper<Vehicle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleWrapper"/> class.
        /// </summary>
        public VehicleWrapper()
            : base(new Vehicle())
        {
        }

        public VehicleWrapper(Vehicle model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public string VehicleModel
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Year
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Patent
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime PatentExpiration
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime DinatranExpiration
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime FireExtinguisherExpiration
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
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
