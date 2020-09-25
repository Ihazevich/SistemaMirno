// <copyright file="ITransferOrderRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface ITransferOrderRepository : IGenericRepository<TransferOrder>
    {
        Task<IEnumerable<WorkUnit>> GetAllWorkUnitsAvailableForTransferAsync(int destinationBranchId);

        Task<IEnumerable<Employee>> GetAllLogisticResponsiblesAsync();

        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();

        Task<IEnumerable<Branch>> GetAllBranchesNotCurrentAsync(int originBranchId);

        void DeleteTransferUnitAsync(TransferUnit transferUnit);

        Task<IEnumerable<WorkArea>> GetTransferWorkAreasAsync(int destinationBranchId);

        Task<IEnumerable<TransferOrder>> GetAllUnconfirmedAsync(int branchId);

        Task<IEnumerable<TransferOrder>> GetAllIncomingAsync(int branchId);
    }
}