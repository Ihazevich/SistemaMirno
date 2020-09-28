// <copyright file="BroadcastSessionInfoEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the event raised when a <see cref="AskSessionInfoEvent"/> is received.
    /// </summary>
    public class BroadcastSessionInfoEvent : PubSubEvent<SessionInfo>
    {
    }
}
