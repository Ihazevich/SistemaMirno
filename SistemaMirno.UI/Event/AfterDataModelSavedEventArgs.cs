// <copyright file="AfterDataModelSavedEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the arguments of the <see cref="AfterDataModelSavedEventArgs"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the deleted model.</typeparam>
    public class AfterDataModelSavedEventArgs<T>
    {
        /// <summary>
        /// Gets or sets the model that triggered the event.
        /// </summary>
        public T Model { get; set; }
    }
}
