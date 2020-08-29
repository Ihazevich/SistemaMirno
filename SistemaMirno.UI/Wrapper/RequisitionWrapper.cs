using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class RequisitionWrapper : ModelWrapper<Requisition>
    {
        public RequisitionWrapper()
            : base(new Requisition())
        {
        }

        public RequisitionWrapper(Requisition model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public DateTime RequestedDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string Priority
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime? TargetDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public bool Fulfilled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public DateTime? FulfilledDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public bool IsForStock
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public int? ClientId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public int? SaleId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(TargetDate):
                    if (TargetDate.HasValue)
                    {
                        if (TargetDate.Value.Date < DateTime.Today.Date)
                        {
                            yield return "El pedido no se puede entregar en el pasado. Todavia >:D";
                        }
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
