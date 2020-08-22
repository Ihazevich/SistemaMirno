// <copyright file="UserWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the User class.
    /// </summary>
    public class UserWrapper : ModelWrapper<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserWrapper"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public UserWrapper(User model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the User ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the User name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the User password.
        /// </summary>
        public string Password
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the User password.
        /// </summary>
        public int EmployeeId
        {
            get { return GetValue<int>(); }
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
                        yield return "El nombre de usuario es muy corto.";
                    }

                    break;

                case nameof(Password):
                    if (Password.Length < 4)
                    {
                        yield return "La contraseña debe contener al menos 4 caracteres";
                    }

                    break;
            }
        }
    }
}
