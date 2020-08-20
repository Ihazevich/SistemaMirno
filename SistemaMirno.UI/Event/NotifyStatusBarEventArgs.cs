// <copyright file="NotifyStatusBarEvent.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    public class NotifyStatusBarEventArgs
    {
        public string Message { get; set; }

        public bool Processing { get; set; }
    }
}