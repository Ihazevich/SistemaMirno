using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public interface IAreaConnectionRepository : IGenericRepository<AreaConnection>
    {
        Task<IEnumerable<AreaConnection>> GetByAreaIdAsync(int areaId);

        Task<string> GetWorkAreaNameAsync(int areaId);
    }
}