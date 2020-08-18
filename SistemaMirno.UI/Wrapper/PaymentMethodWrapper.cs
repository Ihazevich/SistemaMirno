// <copyright file="PaymentMethodWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class PaymentMethodWrapper : ModelWrapper<PaymentMethod>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentMethodWrapper"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public PaymentMethodWrapper(PaymentMethod model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the payment method ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the payment method name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the collection of payments related to the payment method.
        /// </summary>
        public virtual Collection<Payment> Payments
        {
            get { return GetValue<Collection<Payment>>(); }
            set { SetValue(value); }
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
            }
        }
    }
}
