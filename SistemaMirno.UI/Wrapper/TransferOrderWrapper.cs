using System;
using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class TransferOrderWrapper : ModelWrapper<TransferOrder>
    {
        public TransferOrderWrapper()
            : base(new TransferOrder())
        {
        }

        public TransferOrderWrapper(TransferOrder model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public DateTime Date
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int FromBranchId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ToBranchId
        {
            get => GetValue<int>();
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

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ToBranchId):
                    if (ToBranchId < 1)
                    {
                        yield return "Debe seleccionar una sucursal";
                    }

                    break;

                case nameof(VehicleId):
                    if (VehicleId < 1)
                    {
                        yield return "Debe seleccionar un vehiculo";
                    }

                    break;

                case nameof(ResponsibleId):
                    if (ResponsibleId < 1)
                    {
                        yield return "Debe seleccionar un responsable";
                    }

                    break;

                case nameof(Date):
                    if (Date.Year <= DateTime.Today.Year && Date.Month <= DateTime.Today.Month && Date.Day < DateTime.Today.Day)
                    {
                        yield return "La entrega no puede ser en el pasado";
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
