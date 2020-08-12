// <copyright file="UserChangedEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the argument sent with the <see cref="UserChangedEvent"/> event.
    /// </summary>
    public class UserChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the access level of the user.
        /// </summary>
        public int AccessLevel { get; set; }
    }
}
