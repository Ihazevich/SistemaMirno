// <copyright file="NewWorkOrderEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the event triggered when a new work order is made.
    /// </summary>
    /// <typeparam name="T">The type of the saved model.</typeparam>
    public class NewWorkOrderEvent : PubSubEvent<NewWorkOrderEventArgs>
    {
    }
}
