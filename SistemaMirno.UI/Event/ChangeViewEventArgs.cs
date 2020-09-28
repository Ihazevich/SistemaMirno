// <copyright file="ChangeViewEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the arguments passed when the <see cref="ChangeViewEvent"/> is raised.
    /// </summary>
    public class ChangeViewEventArgs
    {
        /// <summary>
        /// Gets or sets the name of the ViewModel that triggered the event.
        /// </summary>
        public string ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the id of the ViewModel.
        /// </summary>
        public int? Id { get; set; }
    }
}