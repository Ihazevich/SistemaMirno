using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IWorkAreaRepository : IGenericRepository<WorkArea>
    {
        Task<List<WorkAreaConnection>> GetAllWorkAreaConnectionsFromWorkAreaAsync(int id);
        Task<List<Branch>> GetAllBranchesAsync();
        Task<List<WorkArea>> GetAllWorkAreasAsync();
        Task<List<Role>> GetAllRolesAsync();
        Task<bool> CheckIfLastExistsAsync(int id);
        Task<bool> CheckIfFirstExistsAsync(int id);
        Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int id);
    }
}