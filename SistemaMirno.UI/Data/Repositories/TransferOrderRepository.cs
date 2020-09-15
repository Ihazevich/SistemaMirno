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
    public class TransferOrderRepository : GenericRepository<TransferOrder, MirnoDbContext>, ITransferOrderRepository
    {
        public TransferOrderRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        public override async Task<TransferOrder> GetByIdAsync(int? id)
        {
            try
            {
                return await Context.TransferOrders.Include(t => t.TransferUnits.Select(u => u.WorkUnit.Product))
                    .Include(t => t.TransferUnits.Select(u => u.WorkUnit.Material))
                    .Include(t => t.TransferUnits.Select(u => u.WorkUnit.Color))
                    .SingleAsync(t => t.Id == id);
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

        public async Task<IEnumerable<WorkUnit>> GetAllWorkUnitsAvailableForTransferAsync(int destinationBranchId)
        {
            try
            {
                // Select only the work units that are currently in areas with the same name as the areas
                // that exist in the the destination branch
                return await Context.WorkUnits.Where(w =>
                    Context.Branches.FirstOrDefault(b => b.Id == destinationBranchId).WorkAreas.Select(a => a.Name)
                        .Contains(w.CurrentWorkArea.Name) && !w.Delivered && !w.Moving).ToListAsync();
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

        public async Task<IEnumerable<Employee>> GetAllLogisticResponsiblesAsync()
        {
            try
            {
                return await Context.Employees.Where(e => e.Roles.Any(r => r.IsFromLogisticsDepartment)).ToListAsync();
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

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            try
            {
                return await Context.Vehicles.ToListAsync();
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

        public async Task<IEnumerable<Branch>> GetAllBranchesNotCurrentAsync(int originBranchId)
        {
            try
            {
                return await Context.Branches.Where(b => b.Id != originBranchId).ToListAsync();
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

        public void DeleteTransferUnitAsync(TransferUnit transferUnit)
        {
            try
            {
                Context.Entry(transferUnit).State = EntityState.Deleted;
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
            }
        }

        public async Task<IEnumerable<WorkArea>> GetTransferWorkAreasAsync(int destinationBranchId)
        {
            try
            {
                return await Context.WorkAreas.Where(w => w.BranchId == destinationBranchId).ToListAsync();
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
