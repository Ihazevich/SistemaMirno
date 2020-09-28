// <copyright file="ChangeViewEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the event raised to change the view in the main view.
    /// </summary>
    public class ChangeViewEvent : PubSubEvent<ChangeViewEventArgs>
    {
    }
}