// <copyright file="ITransferOrderRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Role"/> model.
    /// </summary>
    public interface ITransferOrderRepository : IGenericRepository<TransferOrder>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database that are available
        /// for transfer to a given <see cref="Branch"/>.
        /// </summary>
        /// <param name="destinationBranchId">The id of the destination branch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<WorkUnit>> GetAllWorkUnitsAvailableForTransferAsync(int destinationBranchId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="Employee"/> entities from the database that belong to the logisti department.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<Employee>> GetAllLogisticResponsiblesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Vehicle"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Branch"/> entities from the database except the current one.
        /// </summary>
        /// <param name="originBranchId">The current branch id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<Branch>> GetAllBranchesNotCurrentAsync(int originBranchId);

        /// <summary>
        /// Deletes a <see cref="TransferUnit"/> entity from the database.
        /// </summary>
        /// <param name="transferUnit">The transfer unit.</param>
        void DeleteTransferUnitAsync(TransferUnit transferUnit);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkArea"/> entities from the database that belong to the destinantion branch.
        /// </summary>
        /// <param name="destinationBranchId">The destination branch id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<WorkArea>> GetTransferWorkAreasAsync(int destinationBranchId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="TransferOrder"/> entities from the database that have an unconfirmed status
        /// and belong to the specific branch.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<TransferOrder>> GetAllUnconfirmedAsync(int branchId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="TransferOrder"/> entities from the database that are incoming to a specific branch.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<TransferOrder>> GetAllIncomingAsync(int branchId);
    }
}