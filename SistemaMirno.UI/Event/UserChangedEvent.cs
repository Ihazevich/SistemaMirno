// <copyright file="UserChangedEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the event raised when the current user changed.
    /// </summary>
    public class UserChangedEvent : PubSubEvent<UserChangedEventArgs>
    {
    }
}
