using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public interface IWorkOrderRepository : IGenericRepository<WorkOrder>
    {
        /// <summary>
        /// Gets all the Work Units that match the provided Work Area id.
        /// </summary>
        /// <param name="id">A <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<WorkOrder>> GetByAreaIdAsync(int areaId);

        Task<string> GetWorkAreaNameAsync(int areaId);

        Task<IEnumerable<Product>> GetProductsAsync();

        Task<IEnumerable<Color>> GetColorsAsync();

        Task<IEnumerable<Material>> GetMaterialsAsync();
    }
}