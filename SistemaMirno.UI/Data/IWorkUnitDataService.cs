using SistemaMirno.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public interface IWorkUnitDataService
    {
        Task<IEnumerable<WorkUnit>> GetWorkUnitsByAreaIdAsync(int areaID);
        Task<string> GetProductionAreaName(int areaId);
    }
}