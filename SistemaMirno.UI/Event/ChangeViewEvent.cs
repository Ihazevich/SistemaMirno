// <copyright file="ChangeViewEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the event used to change the view in the main view.
    /// </summary>
    /// <typeparam name="T">The type of the saved model.</typeparam>
    public class ChangeViewEvent : PubSubEvent<ChangeViewEventArgs>
    {
    }
}