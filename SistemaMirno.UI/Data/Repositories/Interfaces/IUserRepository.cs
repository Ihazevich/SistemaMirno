// <copyright file="IUserRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="User"/> model.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        /// <summary>
        /// Asynchronously retrieves a <see cref="User"/> entity from the database
        /// that matches the given username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<User> GetByUsernameAsync(string username);

        /// <summary>
        /// Asynchronously retrieves all <see cref="Role"/> entities from the database
        /// that are related to a specific <see cref="Employee"/>.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Role>> GetAllRolesFromEmployeeAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all <see cref="Employee"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}