// <copyright file="SessionInfo.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Represents the information of the current session.
    /// </summary>
    public struct SessionInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether a user has logged in or not.
        /// </summary>
        public bool UserLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the current user of the session.
        /// </summary>
        public UserWrapper User { get; set; }

        /// <summary>
        /// Gets or sets the current branch of the session.
        /// </summary>
        public BranchWrapper Branch { get; set; }
    }
}