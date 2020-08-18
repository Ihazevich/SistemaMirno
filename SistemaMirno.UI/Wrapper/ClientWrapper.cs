// <copyright file="ClientWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the Client model.
    /// </summary>
    public class ClientWrapper : ModelWrapper<Client>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWrapper"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ClientWrapper(Client model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Client ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the Client's first name.
        /// </summary>
        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Client's last name.
        /// </summary>
        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string RUC
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string PhoneNumber
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Address
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string City
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Department
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public virtual ICollection<Requisition> Requisitions
        {
            get { return GetValue<ICollection<Requisition>>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (FirstName.Length < 2)
                    {
                        yield return "El nombre es muy corto.";
                    }

                    break;
            }
        }
    }
}
