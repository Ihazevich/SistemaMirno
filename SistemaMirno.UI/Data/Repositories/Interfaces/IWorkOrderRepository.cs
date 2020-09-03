// <copyright file="IWorkOrderRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IWorkOrderRepository : IGenericRepository<WorkOrder>
    {
        Task<WorkArea> GetWorkAreaAsync(int workAreaId);

        Task<List<Product>> GetAllProductsAsync();

        Task<List<Material>> GetAllMaterialsAsync();

        Task<List<Color>> GetAllColorsAsync();

        Task<List<WorkUnit>> GetExistingWorkUnits(ICollection<WorkAreaConnection> incomingConnections);

        Task<List<Employee>> GetEmployeesWithRoleIdAsync(int roleId);

        Task<List<WorkUnit>> GetRequisitionWorkUnits();

        Task<List<WorkUnit>> GetWorkUnitsByIdAsync(ICollection<int> idCollection);

        Task<List<WorkOrder>> GetAllWorkOrdersFromWorkAreasBetweenDatesAsync(List<int> workAreasIds, DateTime fromDate, DateTime toDate);

        Task<List<WorkArea>> GetAllWorkAreasAsync(int branchId);
    }
}