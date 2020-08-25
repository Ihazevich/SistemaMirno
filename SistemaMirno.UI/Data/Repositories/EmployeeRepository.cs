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
    }
}
