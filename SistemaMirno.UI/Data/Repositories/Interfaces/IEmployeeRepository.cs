// <copyright file="IEmployeeRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Employee"/> model.
    /// </summary>
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="Branch"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Branch>> GetAllBranchesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Role"/> entities from the database
        /// that are related to a specific <see cref="Branch"/> entity.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Role>> GetAllRolesFromBranchAsync(int branchId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkOrderUnit"/> entities from the database
        /// that are related to a specific <see cref="Employee"/> entity and were made this month.
        /// </summary>
        /// <param name="employeeId">The employee id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkOrderUnit>> GetThisMonthWorkOrderUnitsFromEmployeeAsync(int employeeId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkOrderUnit"/> entities from the database
        /// that are related to a specific <see cref="Employee"/> entity and were made this year.
        /// </summary>
        /// <param name="employeeId">The employee id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkOrderUnit>> GetThisYearsWorkOrderUnitsFromEmployeeAsync(int employeeId);
    }
}