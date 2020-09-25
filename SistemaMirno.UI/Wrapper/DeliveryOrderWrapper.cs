// <copyright file="DeliveryOrderWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class DeliveryOrderWrapper : ModelWrapper<DeliveryOrder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryOrderWrapper"/> class.
        /// </summary>
        public DeliveryOrderWrapper()
            : base(new DeliveryOrder())
        {
        }

        public DeliveryOrderWrapper(DeliveryOrder model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public DateTime Date
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int VehicleId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ResponsibleId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int KmBefore
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int KmAfter
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Date):
                    var today = DateTime.Today;
                    if (Date.Year >= today.Year && Date.Month >= today.Month && Date.Day > today.Day)
                    {
                        yield return "No se puede entregar en el futuro, todavia :P";
                    }

                    break;

                case nameof(ResponsibleId):
                    if (ResponsibleId < 1)
                    {
                        yield return "Debe seleccionar un responsable";
                    }

                    break;

                case nameof(VehicleId):
                    if (VehicleId < 1)
                    {
                        yield return "Debe seleccionar un vehiculo";
                    }

                    break;

                case nameof(KmAfter):
                    if (KmAfter < 0)
                    {
                        yield return "No puede ser negativo.";
                    }

                    break;

                case nameof(KmBefore):
                    if (KmBefore < 0)
                    {
                        yield return "No puede ser negativo.";
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
