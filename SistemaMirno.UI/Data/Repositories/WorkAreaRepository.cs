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
    public class WorkAreaRepository : GenericRepository<WorkArea, MirnoDbContext>, IWorkAreaRepository
    {
        public WorkAreaRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        public async Task<List<Branch>> GetAllBranchesAsync()
        {
            try
            {
                return await Context.Branches.ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        public async Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int id)
        {
            try
            {
                return await Context.WorkAreas.Where(w => w.BranchId == id).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        public async Task<List<Role>> GetAllRolesFromBranchAsync(int id)
        {
            try
            {
                return await Context.Roles.Where(r => r.BranchId == id).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }



        public async Task<List<WorkAreaConnection>> GetWorkAreaConnectionsFromWorkAreaBranchAsync(int id)
        {
            try
            {
                return await Context.WorkAreaConnections.Where(c => c.OriginWorkAreaId == id).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        public async Task<bool> CheckIfFirstExistsAsync()
        {
            try
            {
                return await Context.WorkAreas.AnyAsync(w => w.IsFirst);
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return true;
            }
        }

        public async Task<bool> CheckIfLastExistsAsync()
        {
            try
            {
                return await Context.WorkAreas.AnyAsync(w => w.IsLast);
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return true;
            }
        }
    }
}
