// <copyright file="IUserRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// An interface for the user data repository.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Returns a single user whose name matches the provided value.
        /// </summary>
        /// <param name="name">The name of the user to look for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<User> GetByNameAsync(string name);
    }
}
