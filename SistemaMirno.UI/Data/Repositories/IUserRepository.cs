// <copyright file="IUserRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// An interface for the user data repository.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}
