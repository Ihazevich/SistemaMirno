// <copyright file="BranchChangedEventArgs.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Arguments passed when the <see cref="BranchChangedEvent"/> is raised.
    /// </summary>
    public struct BranchChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the id of the current branch.
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets the name of the current branch.
        /// </summary>
        public string Name { get; set; }
    }
}