using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IDeliveryOrderRepository : IGenericRepository<DeliveryOrder>
    {
        Task<List<Vehicle>> GetAllVehiclesAsync();

        Task<List<Employee>> GetAllLogisticResponsiblesAsync();

        Task<List<Sale>> GetAllNonDeliveredSalesAsync();
    }
}