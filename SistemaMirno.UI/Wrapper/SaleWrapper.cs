using System;
using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class SaleWrapper : ModelWrapper<Sale>
    {
        public SaleWrapper()
            : base(new Sale())
        {
        }

        public SaleWrapper(Sale model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public DateTime Date
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int BranchId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ClientId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ResponsibleId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public DateTime? DeliveryDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public DateTime? EstimatedDeliveryDate
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public long Subtotal
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long Total
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long Discount
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long Tax
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long DeliveryFee
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public bool HasInvoice
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public int? InvoiceId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            var today = DateTime.Today;
            switch (propertyName)
            {
                case nameof(Date):
                    if (Date.Day > today.Day && Date.Month >= today.Month && Date.Year >= today.Year)
                    {
                        yield return "La venta se hizo en el futuro? Solo McFly podria.";
                    }

                    break;

                case nameof(BranchId):
                    if (BranchId < 1)
                    {
                        yield return "Debe seleccionar una sucursal";
                    }

                    break;

                case nameof(ClientId):
                    if (ClientId < 1)
                    {
                        yield return "Debe seleccionar un cliente";
                    }

                    break;

                case nameof(ResponsibleId):
                    if (ResponsibleId < 1)
                    {
                        yield return "Debe seleccionar una vendedor/a";
                    }

                    break;

                case nameof(DeliveryDate):
                    if (DeliveryDate.HasValue)
                    {
                        if (DeliveryDate.Value.Day < today.Day && DeliveryDate.Value.Month <= today.Month && DeliveryDate.Value.Year <= today.Year)
                        {
                            yield return "No podemos entregar en el pasado, todavia >:P.";
                        }
                    }

                    break;

                case nameof(Subtotal):
                    if (Discount >= 0 && DeliveryFee >= 0)
                    {
                        CalculateTotalAndTax();
                    }

                    break;

                case nameof(Total):
                    if (Total < 0)
                    {
                        yield return "Total no puede ser negativo";
                    }

                    break;

                case nameof(Discount):
                    if (Discount < 0)
                    {
                        yield return "Descuento no puede ser negativo, menos por menos es mas!";
                    }
                    else if (DeliveryFee >= 0)
                    {
                        CalculateTotalAndTax();
                    }

                    break;

                case nameof(Tax):
                    if (Tax < 0)
                    {
                        yield return "Ojala los impuestos fueran negativos";
                    }

                    break;

                case nameof(DeliveryFee):
                    if (DeliveryFee < 0)
                    {
                        yield return "Cantidad no puede ser negativa";
                    }
                    else if (Discount >= 0)
                    {
                        CalculateTotalAndTax();
                    }

                    break;

                case nameof(HasInvoice):
                    InvoiceId = 0;
                    break;

                case nameof(InvoiceId):
                    if (HasInvoice && InvoiceId < 0)
                    {
                        yield return "Debe seleccionar una factura";
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

        private void CalculateTotalAndTax()
        {
            Total = Subtotal + DeliveryFee - Discount;
            Tax = Total - (long)(Total / 1.1);
        }
    }
}
