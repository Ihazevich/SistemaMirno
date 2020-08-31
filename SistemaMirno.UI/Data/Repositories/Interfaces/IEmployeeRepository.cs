using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<List<Branch>> GetAllBranchesAsync();

        Task<List<Role>> GetAllRolesFromBranchAsync(int branchId);
        Task<List<WorkOrderUnit>> GetThisMonthWorkOrderUnitsFromEmployeeAsync(int employeeId);
    }
}