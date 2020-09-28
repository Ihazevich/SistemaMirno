// <copyright file="NotifyStatusBarEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the arguments passed when the <see cref="NotifyStatusBarEvent"/> is raised.
    /// </summary>
    public class NotifyStatusBarEventArgs
    {
        /// <summary>
        /// Gets or sets the message of the status bar.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the loading bar is active or not.
        /// </summary>
        public bool Processing { get; set; }
    }
}