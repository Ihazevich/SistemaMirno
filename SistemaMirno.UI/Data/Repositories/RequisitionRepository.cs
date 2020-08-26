using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.Data.Repositories
{
    public class RequisitionRepository : GenericRepository<Requisition, MirnoDbContext>, IRequisitionRepository
    {
        public RequisitionRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator) 
            : base(contextCreator, eventAggregator)
        {
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            try
            {
                return await Context.Clients.ToListAsync();
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }
    }
}
