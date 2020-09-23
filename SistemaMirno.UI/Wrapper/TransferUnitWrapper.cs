using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class TransferUnitWrapper : ModelWrapper<TransferUnit>
    {
        public TransferUnitWrapper()
            : base(new TransferUnit())
        {
        }

        public TransferUnitWrapper(TransferUnit model)
            : base(model)
        {
        }
        
        public int TransferOrderId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int WorkUnitId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int FromWorkAreaId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ToWorkAreaId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool Lost
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool Cancelled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool Arrived
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Lost):
                    if (Lost)
                    {
                        Cancelled = false;
                        Arrived = false;
                    }

                    break;

                case nameof(Cancelled):
                    if (Cancelled)
                    {
                        Lost = false;
                        Arrived = false;
                    }

                    break;

                case nameof(Arrived):
                    if (Arrived)
                    {
                        Cancelled = false;
                        Lost = false;
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
