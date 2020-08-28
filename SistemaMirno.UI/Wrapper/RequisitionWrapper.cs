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

        public int Id
        {
            get { return GetValue<int>(); }
        }

        public DateTime RequestedDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public string Priority
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime? TargetDate
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public bool Fulfilled
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public DateTime? FulfilledDate
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public bool IsForStock
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public int? ClientId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }

        public int? SaleId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
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
        }
    }
}
