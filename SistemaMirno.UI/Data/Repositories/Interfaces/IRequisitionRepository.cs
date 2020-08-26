using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IRequisitionRepository : IGenericRepository<Requisition>
    {
        Task<List<Client>> GetAllClientsAsync();
    }
}