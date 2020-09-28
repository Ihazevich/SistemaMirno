// <copyright file="IWorkOrderRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="WorkOrder"/> model.
    /// </summary>
    public interface IWorkOrderRepository : IGenericRepository<WorkOrder>
    {
        /// <summary>
        /// Asynchronously retrieves a <see cref="WorkArea"/> entity from the database
        /// that matches the given id.
        /// </summary>
        /// <param name="workAreaId">The id of the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<WorkArea> GetWorkAreaAsync(int workAreaId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="Product"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Product>> GetAllProductsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Material"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Material>> GetAllMaterialsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Color"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Color>> GetAllColorsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database
        /// that are currently in work areas matching the given connections.
        /// </summary>
        /// <param name="incomingConnections">The list of incoming connections to the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetExistingWorkUnits(ICollection<WorkAreaConnection> incomingConnections);

        /// <summary>
        /// Asynchronously retrieves all <see cref="Employee"/> entities from the database
        /// that have the matching role.
        /// </summary>
        /// <param name="roleId">The id of the role.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Employee>> GetEmployeesWithRoleIdAsync(int roleId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database
        /// that are currently in the first(requisition) work area.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetRequisitionWorkUnits();

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database
        /// that match the provided ids.
        /// </summary>
        /// <param name="idCollection">The collection of ids.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetWorkUnitsByIdAsync(ICollection<int> idCollection);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkOrder"/> entities from the database
        /// created between two dates and originated from a list of work areas.
        /// </summary>
        /// <param name="workAreasIds">The list of work areas ids</param>
        /// <param name="fromDate">The start date.</param>
        /// <param name="toDate">The end date.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkOrder>> GetAllWorkOrdersFromWorkAreasBetweenDatesAsync(List<int> workAreasIds, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkArea"/> entities from the database
        /// that belong to a given branch.
        /// </summary>
        /// <param name="branchId">The id of the branch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkArea>> GetAllWorkAreasAsync(int branchId);
    }
}