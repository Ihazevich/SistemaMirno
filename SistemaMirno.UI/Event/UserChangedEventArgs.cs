// <copyright file="UserChangedEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// A struct representing the argument sent with the <see cref="UserChangedEvent"/> event.
    /// </summary>
    public struct UserChangedEventArgs
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string EmployeeFullName { get; set; }
        public bool IsSystemAdmin { get; set; }
        public bool HasAccessToSales { get; set; }
        public bool HasAccessToAccounting { get; set; }
        public bool HasAccessToProduction { get; set; }
        public bool HasAccessToLogistics { get; set; }
        public bool HasAccessToHumanResources { get; set; }
    }
}
