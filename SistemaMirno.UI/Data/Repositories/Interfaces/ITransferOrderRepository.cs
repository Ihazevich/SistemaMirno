using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

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
    }
}