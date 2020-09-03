// <copyright file="SessionInfo.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Event
{
    public struct SessionInfo
    {
        public bool UserLoggedIn { get; set; }

        public UserWrapper User { get; set; }

        public BranchWrapper Branch { get; set; }
    }
}