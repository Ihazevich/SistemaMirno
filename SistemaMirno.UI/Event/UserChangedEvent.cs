// <copyright file="UserChangedEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the event raised when the system user changed.
    /// </summary>
    public class UserChangedEvent : PubSubEvent<UserChangedEventArgs>
    {
    }
}
