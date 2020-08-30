// <copyright file="NewWorkOrderEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the arguments of the <see cref="NewWorkOrderEvent"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the deleted model.</typeparam>
    public class NewWorkOrderEventArgs
    {
        /// <summary>
        /// Gets or sets the collection of WorkUnits depending on the work order that triggered the event.
        /// </summary>
        public IReadOnlyCollection<WorkUnitWrapper> WorkUnits { get; set; }

        /// <summary>
        /// Gets or sets the id of the work area where the work order originated.
        /// </summary>
        public int OriginWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the id of the work area the work order is destined to.
        /// </summary>
        public int DestinationWorkAreaId { get; set; }
    }
}
