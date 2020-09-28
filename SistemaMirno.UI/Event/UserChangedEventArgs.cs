// <copyright file="UserChangedEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the arguments passed when the <see cref="UserChangedEvent"/> is raised.
    /// </summary>
    public struct UserChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the new user.
        /// </summary>
        public UserWrapper User { get; set; }
    }
}
