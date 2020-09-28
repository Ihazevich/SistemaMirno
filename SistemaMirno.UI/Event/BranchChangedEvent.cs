// <copyright file="BranchChangedEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the event raised when the current branch changed.
    /// </summary>
    public class BranchChangedEvent : PubSubEvent<BranchChangedEventArgs>
    {
    }
}
