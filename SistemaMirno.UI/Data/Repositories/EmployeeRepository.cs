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
    public class EmployeeRepository : GenericRepository<Employee, MirnoDbContext>, IEmployeeRepository
    {
        public EmployeeRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        public Task<List<Branch>> GetAllBranchesAsync()
        {
            return Context.Branches.ToListAsync();
        }

        public Task<List<Role>> GetAllRolesFromBranchAsync(int branchId)
        {
            return Context.Roles.Where(r => r.BranchId == branchId).ToListAsync();
        }

        public async Task<List<WorkOrderUnit>> GetThisMonthWorkOrderUnitsFromEmployeeAsync(int employeeId)
        {
            try
            {
                return await Context.WorkOrderUnits.Where(w =>
                    w.WorkOrder.ResponsibleEmployeeId == employeeId &&
                    w.WorkOrder.CreationDateTime.Year == DateTime.Today.Year &&
                    w.WorkOrder.CreationDateTime.Month == DateTime.Today.Month)
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

        public async Task<List<WorkOrderUnit>> GetThisYearsWorkOrderUnitsFromEmployeeAsync(int employeeId)
        {
            try
            {
                return await Context.WorkOrderUnits.Where(w =>
                    w.WorkOrder.ResponsibleEmployeeId == employeeId &&
                    w.WorkOrder.CreationDateTime.Year == DateTime.Today.Year)
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
    }
}
