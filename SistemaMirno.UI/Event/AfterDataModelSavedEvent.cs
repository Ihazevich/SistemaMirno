// <copyright file="AfterDataModelSavedEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the event triggered when a data model is saved.
    /// </summary>
    /// <typeparam name="T">The type of the saved model.</typeparam>
    public class AfterDataModelSavedEvent<T> : PubSubEvent<AfterDataModelSavedEventArgs<T>>
    {
    }
}
