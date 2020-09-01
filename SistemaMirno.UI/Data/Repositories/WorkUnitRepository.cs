using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.Data.Repositories
{
    public class WorkUnitRepository : GenericRepository<WorkUnit, MirnoDbContext>, IWorkUnitRepository
    {
        public WorkUnitRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        public async Task<List<WorkUnit>> GetAllWorkUnitsCurrentlyInWorkAreaAsync(int id)
        {
            try
            {
                return await Context.WorkUnits.Where(w => w.CurrentWorkAreaId == id && !w.Sold && !w.Delivered).ToListAsync();
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

        public async Task<WorkArea> GetWorkAreaById(int id)
        {
            try
            {
                return await Context.WorkAreas.FindAsync(id);
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

        public async Task<List<WorkUnit>> GetWorkUnitsInProcessAsync()
        {
            try
            {
                return await Context.WorkUnits.Where(w => !w.Delivered && !w.Sold && w.CurrentWorkArea.ReportsInProcess)
                    .ToListAsync();
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

        public async Task<List<WorkArea>> GetWorkAreasThatReportInProcess()
        {
            try
            {
                return await Context.WorkAreas.Where(w => w.ReportsInProcess).ToListAsync();
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

        public async Task<List<WorkAreaConnection>> GetWorkAreaOutgoingConnections(int workAreaId)
        {
            try
            {
                return await Context.WorkAreaConnections.Where(c => c.OriginWorkAreaId == workAreaId).ToListAsync();
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

        public async Task<List<WorkUnit>> GetAllWorkUnitsInAllLastWorkAreasAsync()
        {
            try
            {
                return await Context.WorkUnits.Where(w => w.CurrentWorkArea.IsLast && !w.Sold && !w.Delivered)
                    .ToListAsync();
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

        public async Task<WorkArea> GetLastWorkAreaFromBranchIdAsync(int id)
        {
            try
            {
                return await Context.WorkAreas.SingleAsync(w => w.IsLast && w.BranchId == id);
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

        public Task<int> FindProductByNameAsync(string workUnitProduct)
        {
            throw new NotImplementedException();
        }

        public Task<int> FindMaterialByNameAsync(string workUnitMaterial)
        {
            throw new NotImplementedException();
        }

        public Task<int> FindColorByNameAsync(string workUnitColor)
        {
            throw new NotImplementedException();
        }

        public Task<int> FindWorkAreaByNameAndBranchNameAsync(string workUnitCurrentArea, string workUnitBranch)
        {
            throw new NotImplementedException();
        }
    }
}
