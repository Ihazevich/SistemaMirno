// <copyright file="ChangeViewEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the arguments of the <see cref="ChangeViewEvent"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the deleted model.</typeparam>
    public class ChangeViewEventArgs
    {
        /// <summary>
        /// Gets or sets the name of the ViewModel that triggered the event.
        /// </summary>
        public string ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the id of the ViewModel.
        /// </summary>
        public int Id { get; set; }
    }
}