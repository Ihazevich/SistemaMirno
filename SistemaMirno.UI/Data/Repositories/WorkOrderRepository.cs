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
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Data.Repositories
{
    public class WorkOrderRepository : GenericRepository<WorkOrder, MirnoDbContext>, IWorkOrderRepository
    {
        public WorkOrderRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator) 
            : base(contextCreator, eventAggregator)
        {
        }

        public async Task<WorkArea> GetWorkAreaAsync(int id)
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


        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await Context.Products.ToListAsync();
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

        public async Task<List<Color>> GetAllColorsAsync()
        {
            try
            {
                return await Context.Colors.ToListAsync();
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

        public async Task<List<Material>> GetAllMaterialsAsync()
        {
            try
            {
                return await Context.Materials.ToListAsync();
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

        public async Task<List<WorkUnit>> GetExistingWorkUnits(ICollection<WorkAreaConnection> incomingConnections)
        {
            var workAreasIds = incomingConnections.Select(c => c.Id);

            try
            {
                return await Context.WorkUnits.Where(w => workAreasIds.Contains(w.CurrentWorkAreaId) && !w.CurrentWorkArea.IsFirst).ToListAsync();
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

        public async Task<List<WorkUnit>> GetRequisitionWorkUnits()
        {
            try
            {
                return await Context.WorkUnits.Where(w => w.CurrentWorkArea.IsFirst).ToListAsync();
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

        public async Task<List<Employee>> GetEmployeesWithRoleIdAsync(int roleId)
        {
            try
            {
                return await Context.Employees.Where(e => e.Roles.Select(r => r.Id).Contains(roleId))
                    .ToListAsync();
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

        public async Task<List<WorkUnit>> GetWorkUnitsByIdAsync(ICollection<int> idCollection)
        {
            try
            {
                return await Context.WorkUnits.Where(w => idCollection.Contains(w.Id)).ToListAsync();
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

        public async Task<List<WorkOrder>> GetAllWorkOrdersFromWorkAreasBetweenDatesAsync(List<int> workAreasIds, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await Context.WorkOrders.Where(w =>
                    workAreasIds.Contains(w.DestinationWorkAreaId) && w.CreationDateTime.Year >= fromDate.Year &&
                    w.CreationDateTime.Month >= fromDate.Month && w.CreationDateTime.Day >= fromDate.Day &&
                    w.CreationDateTime.Year <= toDate.Year && w.CreationDateTime.Month <= toDate.Month &&
                    w.CreationDateTime.Day <= toDate.Day).ToListAsync();
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

        public async Task<List<WorkArea>> GetAllWorkAreasAsync(int branchId)
        {
            try
            {
                return await Context.WorkAreas.Where(w => w.BranchId == branchId && !w.IsLast && !w.IsFirst)
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
    }
}
