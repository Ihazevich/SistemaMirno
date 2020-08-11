// <copyright file="AfterDataModelDeletedEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using Prism.Events;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A class representing the event triggered when a data model is deleted.
    /// </summary>
    /// <typeparam name="T">The type of the saved model.</typeparam>
    public class AfterDataModelDeletedEvent<T> : PubSubEvent<AfterDataModelDeletedEventArgs<T>>
    {
    }
}
