// <copyright file="ChangeNavigationStatusEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the event raised to change the navigation status of the menu
    /// and side navigation bar.
    /// </summary>
    public class ChangeNavigationStatusEvent : PubSubEvent<bool>
    {
    }
}
