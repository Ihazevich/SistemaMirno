// <copyright file="UserChangedEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A struct representing the argument sent with the <see cref="UserChangedEvent"/> event.
    /// </summary>
    public struct UserChangedEventArgs
    {
        public UserWrapper User { get; set; }
    }
}
