// <copyright file="BranchChangedEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    public struct BranchChangedEventArgs
    {
        public int BranchId { get; set; }

        public string Name { get; set; }
    }
}