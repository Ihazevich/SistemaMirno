using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IWorkAreaRepository : IGenericRepository<WorkArea>
    {
        Task<List<WorkAreaConnection>> GetWorkAreaConnectionsFromWorkAreaBranchAsync(int id);
        Task<List<Branch>> GetAllBranchesAsync();
        Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int id);
        Task<List<Role>> GetAllRolesFromBranchAsync(int id);
        Task<bool> CheckIfLastExistsAsync(int id);
        Task<bool> CheckIfFirstExistsAsync(int id);
    }
}