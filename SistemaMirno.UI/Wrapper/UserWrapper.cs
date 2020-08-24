// <copyright file="UserWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the User class.
    /// </summary>
    public class UserWrapper : ModelWrapper<User>
    {
        public UserWrapper()
            : base(new User())
        {

        }

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
        public string Username
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
                case nameof(Username):
                    if (Username.Length < 4)
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

        public string GetPasswordHash(string password)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = Encoding.UTF8.GetBytes(password);
                var generatedHash = sha1.ComputeHash(hash);
                var generatedHashString = Convert.ToBase64String(generatedHash);
                return generatedHashString;
            }
        }
    }
}
