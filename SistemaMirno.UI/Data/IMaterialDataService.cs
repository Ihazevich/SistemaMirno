using SistemaMirno.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public interface IMaterialDataService
    {
        Task<List<Material>> GetAllAsync();
    }
}