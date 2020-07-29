using SistemaMirno.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public interface IProductionAreaDataService
    {
        Task<List<ProductionArea>> GetAllAsync();
        Task SaveAsync(ProductionArea area);
    }
}