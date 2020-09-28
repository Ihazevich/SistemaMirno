// <copyright file="ShowDialogEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the arguments passed when the <see cref="ShowDialogEvent"/> is raised.
    /// </summary>
    public class ShowDialogEventArgs
    {
        /// <summary>
        /// Gets or sets the message of the dialog box.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the title of the dialog box.
        /// </summary>
        public string Title { get; set; }
    }
}
