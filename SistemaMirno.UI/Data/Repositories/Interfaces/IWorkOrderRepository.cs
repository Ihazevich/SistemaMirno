using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IWorkOrderRepository : IGenericRepository<WorkOrder>
    {
        Task<WorkArea> GetWorkAreaAsync(int originWorkAreaId);
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Material>> GetAllMaterialsAsync();
        Task<List<Color>> GetAllColorsAsync();
        Task<List<WorkUnit>> GetExistingWorkUnits(ICollection<WorkAreaConnection> modelIncomingConnections);
    }
}