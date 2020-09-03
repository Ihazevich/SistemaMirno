// <copyright file="IEmployeeRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<List<Branch>> GetAllBranchesAsync();

        Task<List<Role>> GetAllRolesFromBranchAsync(int branchId);

        Task<List<WorkOrderUnit>> GetThisMonthWorkOrderUnitsFromEmployeeAsync(int employeeId);

        Task<List<WorkOrderUnit>> GetThisYearsWorkOrderUnitsFromEmployeeAsync(int employeeId);
    }
}