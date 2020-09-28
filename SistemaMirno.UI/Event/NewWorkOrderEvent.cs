// <copyright file="NewWorkOrderEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the event raised to make a new work order.
    /// </summary>
    public class NewWorkOrderEvent : PubSubEvent<NewWorkOrderEventArgs>
    {
    }
}
