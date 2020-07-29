using SistemaMirno.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public interface IProductDataService
    {
        Task<List<Product>> GetAllAsync();
    }
}