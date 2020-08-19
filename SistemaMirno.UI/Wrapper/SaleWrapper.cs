// <copyright file="SaleWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class SaleWrapper : ModelWrapper<Sale>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="ClientWrapper"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public SaleWrapper(Sale model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        public int RequisitionId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual Requisition Requisition
        {
            get { return GetValue<Requisition>(); }
            set { SetValue(value); }
        }

        public int ResponsibleId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual Employee Responsible
        {
            get { return GetValue<Employee>(); }
            set { SetValue(value); }
        }

        public DateTime DeliveryDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public DateTime EstimatedDeliveryDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public int Discount
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int IVA
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int Total
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Type
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public virtual ICollection<Payment> Payments
        {
            get { return GetValue<ICollection<Payment>>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}
